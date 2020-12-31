using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http.Features;

namespace ApplicationIdentity
{
    public class ApplicationAuthorizeFilter : IAsyncAuthorizationFilter, IFilterFactory
    {
        #region 构造函数
        /// <summary>
        /// 初始化授权过滤器
        /// Initializes a new <see cref="AuthorizeFilter"/> instance.
        /// </summary>
        public ApplicationAuthorizeFilter()
            : this(authorizeData: new[] { new AuthorizeAttribute() })
        {
        }

        /// <summary>
        /// 初始化授权过滤器
        /// Initialize a new <see cref="AuthorizeFilter"/> instance.
        /// </summary>
        /// <param name="policy">要使用的授权策略 Authorization policy to be used.</param>
        public ApplicationAuthorizeFilter(AuthorizationPolicy policy)
        {
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            Policy = policy;
        }

        /// <summary>
        /// 初始化授权过滤器
        /// Initialize a new <see cref="AuthorizeFilter"/> instance.
        /// </summary>
        /// <param name="policyProvider"> 授权策略提供程序 The <see cref="IAuthorizationPolicyProvider"/> to use to resolve policy names.</param>
        /// <param name="authorizeData"> 授权数据 The <see cref="IAuthorizeData"/> to combine into an <see cref="IAuthorizeData"/>.</param>
        public ApplicationAuthorizeFilter(IAuthorizationPolicyProvider policyProvider, IEnumerable<IAuthorizeData> authorizeData)
            : this(authorizeData)
        {
            if (policyProvider == null)
            {
                throw new ArgumentNullException(nameof(policyProvider));
            }

            PolicyProvider = policyProvider;
        }

        /// <summary>
        /// 初始化授权过滤器
        /// Initializes a new instance of <see cref="AuthorizeFilter"/>.
        /// </summary>
        /// <param name="authorizeData">授权数据 The <see cref="IAuthorizeData"/> to combine into an <see cref="IAuthorizeData"/>.</param>
        public ApplicationAuthorizeFilter(IEnumerable<IAuthorizeData> authorizeData)
        {
            if (authorizeData == null)
            {
                throw new ArgumentNullException(nameof(authorizeData));
            }

            AuthorizeData = authorizeData;
        }

        /// <summary>
        /// 初始化授权过滤器
        /// Initializes a new instance of <see cref="AuthorizeFilter"/>.
        /// </summary>
        /// <param name="policy">要使用的授权策略 The name of the policy to require for authorization.</param>
        public ApplicationAuthorizeFilter(string policy)
            : this(new[] { new AuthorizeAttribute(policy) })
        {
        }
        #endregion

        #region 属性
        /// <summary>
        /// 授权策略提供程序
        /// The <see cref="IAuthorizationPolicyProvider"/> to use to resolve policy names.
        /// </summary>
        public IAuthorizationPolicyProvider PolicyProvider { get; }

        /// <summary>
        /// 授权数据
        /// The <see cref="IAuthorizeData"/> to combine into an <see cref="IAuthorizeData"/>.
        /// </summary>
        public IEnumerable<IAuthorizeData> AuthorizeData { get; }

        /// <summary>
        /// 要使用的授权策略
        /// </summary>
        /// <remarks>
        /// If<c>null</c>, the policy will be constructed using
        /// <see cref="AuthorizationPolicy.CombineAsync(IAuthorizationPolicyProvider, IEnumerable{IAuthorizeData})"/>.
        /// </remarks>
        public AuthorizationPolicy Policy { get; }
        #endregion

        bool IFilterFactory.IsReusable => true;

        // Computes the actual policy for this filter using either Policy or PolicyProvider + AuthorizeData
        /// <summary>
        /// 使用Policy属性或策略提供程序+授权数据计算该过滤器实际的策略
        /// </summary>
        /// <returns></returns>
        private Task<AuthorizationPolicy> ComputePolicyAsync()
        {
            // Policy属性不为空，直接返回
            if (Policy != null)
            {
                return Task.FromResult(Policy);
            }

            // 策略提供程序为空，抛出异常
            if (PolicyProvider == null)
            {
                //throw new InvalidOperationException(
                //    ResourceSet.FormatAuthorizeFilter_AuthorizationPolicyCannotBeCreated(
                //        nameof(AuthorizationPolicy),
                //        nameof(IAuthorizationPolicyProvider)));
            }

            // 通过策略提供程序+授权数据 计算策略
            return AuthorizationPolicy.CombineAsync(PolicyProvider, AuthorizeData);
        }

        /// <summary>
        /// 异步获取有效的授权策略
        /// </summary>
        /// <param name="context">授权过滤器上下文</param>
        /// <returns></returns>
        internal async Task<AuthorizationPolicy> GetEffectivePolicyAsync(AuthorizationFilterContext context)
        {
            // 将所有授权筛选器组合到单个有效策略中，它只在最近的过滤器上运行
            // Combine all authorize filters into single effective policy that's only run on the closest filter
            var builder = new AuthorizationPolicyBuilder(await ComputePolicyAsync());
            for (var i = 0; i < context.Filters.Count; i++)
            {
                if (ReferenceEquals(this, context.Filters[i]))
                {
                    continue;
                }

                if (context.Filters[i] is ApplicationAuthorizeFilter authorizeFilter)
                {
                    // Combine using the explicit policy, or the dynamic policy provider
                    builder.Combine(await authorizeFilter.ComputePolicyAsync());
                }
            }

            // 获取端点路由的功能接口。
            // 使用Microsoft.AspNetCore.Http.HttpContext.Features访问与当前请求关联的实例。
            var endpoint = context.HttpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint != null)
            {
                // When doing endpoint routing, MVC does not create filters for any authorization specific metadata i.e [Authorize] does not
                // get translated into AuthorizeFilter. Consequently, there are some rough edges when an application uses a mix of AuthorizeFilter
                // explicilty configured by the user (e.g. global auth filter), and uses endpoint metadata.
                // To keep the behavior of AuthFilter identical to pre-endpoint routing, we will gather auth data from endpoint metadata
                // and produce a policy using this. This would mean we would have effectively run some auth twice, but it maintains compat.
                var policyProvider = PolicyProvider ?? context.HttpContext.RequestServices.GetRequiredService<IAuthorizationPolicyProvider>();
                var endpointAuthorizeData = endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>() ?? Array.Empty<IAuthorizeData>();

                var endpointPolicy = await AuthorizationPolicy.CombineAsync(policyProvider, endpointAuthorizeData);
                if (endpointPolicy != null)
                {
                    builder.Combine(endpointPolicy);
                }
            }

            return builder.Build();
        }

        /// <inheritdoc />
        public virtual async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.IsEffectivePolicy(this))
            {
                return;
            }

            // 异步获取有效策略
            // 对授权逻辑的更改应该反映在安全的授权中间件中
            // IMPORTANT: Changes to authorization logic should be mirrored in security's AuthorizationMiddleware
            var effectivePolicy = await GetEffectivePolicyAsync(context);
            if (effectivePolicy == null)
            {
                return;
            }

            // 获取策略评估服务
            var policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();

            // 获得身份验证结果
            var authenticateResult = await policyEvaluator.AuthenticateAsync(effectivePolicy, context.HttpContext);

            // 允许匿名跳过所有授权 Allow Anonymous skips all authorization
            if (HasAllowAnonymous(context))
            {
                return;
            }

            // 获得授权结果
            var authorizeResult = await policyEvaluator.AuthorizeAsync(effectivePolicy, authenticateResult, context.HttpContext, context);

            if (authorizeResult.Challenged)
            {
                context.Result = new ChallengeResult(effectivePolicy.AuthenticationSchemes.ToArray());
            }
            else if (authorizeResult.Forbidden)
            {
                context.Result = new ForbidResult(effectivePolicy.AuthenticationSchemes.ToArray());
            }
        }

        /// <summary>
        /// 创建授权过滤器中间件
        /// </summary>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <returns>过滤器中间件</returns>
        IFilterMetadata IFilterFactory.CreateInstance(IServiceProvider serviceProvider)
        {
            if (Policy != null || PolicyProvider != null)
            {
                // The filter is fully constructed. Use the current instance to authorize.
                return this;
            }

            Debug.Assert(AuthorizeData != null);
            // 获取授权策略提供程序
            var policyProvider = serviceProvider.GetRequiredService<IAuthorizationPolicyProvider>();
            return AuthorizationApplicationModelProvider.GetFilter(policyProvider, AuthorizeData);
        }

        /// <summary>
        /// 已经允许匿名
        /// </summary>
        /// <param name="context">授权验证过滤器上下文</param>
        /// <returns></returns>
        private static bool HasAllowAnonymous(AuthorizationFilterContext context)
        {
            var filters = context.Filters;
            for (var i = 0; i < filters.Count; i++)
            {
                if (filters[i] is IAllowAnonymousFilter)
                {
                    return true;
                }
            }

            /***
             * 当做端点路由时，MVC不会为AllowAnonymousAttributes添加AllowAnonymousFilters
             * 在控制器和操作中被发现。为了保持与2.x的一致性，
             * 我们将检查端点元数据中是否存在IAllowAnonymous。
             * */

            // When doing endpoint routing, MVC does not add AllowAnonymousFilters for AllowAnonymousAttributes that
            // were discovered on controllers and actions. To maintain compat with 2.x,
            // we'll check for the presence of IAllowAnonymous in endpoint metadata.

            // 获取端点路由的功能接口。
            // 使用Microsoft.AspNetCore.Http.HttpContext.Features访问与当前请求关联的实例。
            var endpoint = context.HttpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return true;
            }

            return false;
        }
    }
}
