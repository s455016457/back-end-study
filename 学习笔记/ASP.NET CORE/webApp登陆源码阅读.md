# WebApp登陆源码阅读

## 源码

在 ASP .NET CORE webApp示例代码中，登陆成功后通过`HttpContext.SignInAsync`方法，将身份信息保存在Cookie中。示例代码如下：

```C#
[HttpPost]
public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
{
    ViewData["ReturnUrl"] = returnUrl;

    // 验证用户名和密码
    if (ValidateLogin(userName, password))
    {
        // 创建申明
        var claims = new List<Claim>
        {
            new Claim("user", userName),
            new Claim("role", "Member")
        };

        // 创建身份申明，并添加进cookie中
        await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));

        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return Redirect("/");
        }
    }

    return View();
}
```

`HttpContext.SignInAsync`是在[`AuthenticationHttpContextExtensions`](https://sourcegraph.com/github.com/dotnet/aspnetcore/-/blob/src/Http/Authentication.Abstractions/src/AuthenticationHttpContextExtensions.cs#L135)类中定义的扩展方法，源码为：

```C#
        /// <summary>
        /// Sign in a principal for the specified scheme.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> context.</param>
        /// <param name="scheme">The name of the authentication scheme.</param>
        /// <param name="principal">The user.</param>
        /// <param name="properties">The <see cref="AuthenticationProperties"/> properties.</param>
        /// <returns>The task.</returns>
        public static Task SignInAsync(this HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties) =>
            context.RequestServices.GetRequiredService<IAuthenticationService>().SignInAsync(context, scheme, principal, properties);
```

实际是调用`IAuthenticationService`接口的`SignInAsync`方法，那么`IAuthenticationService`的实现类是哪个呢，示例代码在`StartUp`类的`ConfigureServices`方法中配置了身份验证服务，就是一下代码：

```C#
services.AddAuthentication(CookieScheme) // Sets the default scheme to cookies
        .AddCookie(CookieScheme, options =>
        {
            options.AccessDeniedPath = "/account/denied";
            options.LoginPath = "/account/login";
        });
```

`services.AddAuthentication`调用的是[`AuthenticationCoreServiceCollectionExtensions`](https://sourcegraph.com/github.com/dotnet/aspnetcore@b0a6755b5ef103b57394f115613b45a37912600a/-/blob/src/Http/Authentication.Core/src/AuthenticationCoreServiceCollectionExtensions.cs#L33)扩展类的方法，添加身份验证配置，源码为：

```C#
/// <summary>
/// Add core authentication services needed for <see cref="IAuthenticationService"/>.
/// </summary>
/// <param name="services">The <see cref="IServiceCollection"/>.</param>
/// <returns>The service collection.</returns>
public static IServiceCollection AddAuthenticationCore(this IServiceCollection services)
{
    if (services == null)
    {
        throw new ArgumentNullException(nameof(services));
    }

    // 配置身份验证服务实现类
    services.TryAddScoped<IAuthenticationService, AuthenticationService>();
    services.TryAddSingleton<IClaimsTransformation, NoopClaimsTransformation>(); // Can be replaced with scoped ones that use DbContext
    // 配置身份验证处理程序提供程序
    services.TryAddScoped<IAuthenticationHandlerProvider, AuthenticationHandlerProvider>();
    // 配置身份验证方案提供程序
    services.TryAddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();
    return services;
}
```

从该端代码可知在登陆成功够调用的`HttpContext.SignInAsync`方法,实际调用的是[`AuthenticationService`](https://sourcegraph.com/github.com/dotnet/aspnetcore@b0a6755b5ef103b57394f115613b45a37912600a/-/blob/src/Http/Authentication.Core/src/AuthenticationService.cs#L167)类的`SignInAsync`方法。该方法的源码为：

```C#
/// <summary>
/// Sign a principal in for the specified authentication scheme.
/// </summary>
/// <param name="context">The <see cref="HttpContext"/>.</param>
/// <param name="scheme">The name of the authentication scheme.</param>
/// <param name="principal">The <see cref="ClaimsPrincipal"/> to sign in.</param>
/// <param name="properties">The <see cref="AuthenticationProperties"/>.</param>
/// <returns>A task.</returns>
public virtual async Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
{
    if (principal == null)
    {
        throw new ArgumentNullException(nameof(principal));
    }

    if (Options.RequireAuthenticatedSignIn)
    {
        if (principal.Identity == null)
        {
            throw new InvalidOperationException("SignInAsync when principal.Identity == null is not allowed when AuthenticationOptions.RequireAuthenticatedSignIn is true.");
        }
        if (!principal.Identity.IsAuthenticated)
        {
            throw new InvalidOperationException("SignInAsync when principal.Identity.IsAuthenticated is false is not allowed when AuthenticationOptions.RequireAuthenticatedSignIn is true.");
        }
    }

    if (scheme == null)
    {
        var defaultScheme = await Schemes.GetDefaultSignInSchemeAsync();
        scheme = defaultScheme?.Name;
        if (scheme == null)
        {
            throw new InvalidOperationException($"No authenticationScheme was specified, and there was no DefaultSignInScheme found. The default schemes can be set using either AddAuthentication(string defaultScheme) or AddAuthentication(Action<AuthenticationOptions> configureOptions).");
        }
    }

    // 关键代码  获得身份验证处理程序
    var handler = await Handlers.GetHandlerAsync(context, scheme);
    if (handler == null)
    {
        throw await CreateMissingSignInHandlerException(scheme);
    }

    var signInHandler = handler as IAuthenticationSignInHandler;
    if (signInHandler == null)
    {
        throw await CreateMismatchedSignInHandlerException(scheme, handler);
    }

    await signInHandler.SignInAsync(principal, properties);
}
```

`Handlers`实际是`IAuthenticationHandlerProvider`在`AuthenticationService`的构造函数中传入该值，构造函数源码：

```C#
/// <summary>
/// Constructor.
/// </summary>
/// <param name="schemes">身份验证方案 <see cref="IAuthenticationSchemeProvider"/>.</param>
/// <param name="handlers">身份验证处理提供程序<see cref="IAuthenticationHandlerProvider"/>.</param>
/// <param name="transform">声明转换<see cref="IClaimsTransformation"/>.</param>
/// <param name="options">身份验证选项 <see cref="AuthenticationOptions"/>.</param>
public AuthenticationService(IAuthenticationSchemeProvider schemes, IAuthenticationHandlerProvider handlers, IClaimsTransformation transform, IOptions<AuthenticationOptions> options)
{
    Schemes = schemes;
    Handlers = handlers;
    Transform = transform;
    Options = options.Value;
}
```

那么`IAuthenticationHandlerProvider`接口的实现类是什么呢？可以在上面的源码中看到，在`AuthenticationCoreServiceCollectionExtensions`类的扩展方法`AddAuthenticationCore`中配置了`IAuthenticationHandlerProvider`接口的实现类为[`AuthenticationHandlerProvider`](https://sourcegraph.com/github.com/dotnet/aspnetcore@b0a6755b5ef103b57394f115613b45a37912600a/-/blob/src/Http/Authentication.Core/src/AuthenticationHandlerProvider.cs#L40:1)，`GetHandlerAsync`方法的源码为：

```C#
/// <summary>
/// Returns the handler instance that will be used.
/// </summary>
/// <param name="context">The context.</param>
/// <param name="authenticationScheme">The name of the authentication scheme being handled.</param>
/// <returns>The handler instance.</returns>
public async Task<IAuthenticationHandler?> GetHandlerAsync(HttpContext context, string authenticationScheme)
{
    // 如果内部缓存中存在该方案名称身份验证处理程序，则直接返回处理程序
    if (_handlerMap.TryGetValue(authenticationScheme, out var value))
    {
        return value;
    }

    var scheme = await Schemes.GetSchemeAsync(authenticationScheme);
    if (scheme == null)
    {
        return null;
    }

    // 获得身份验证处理程序
    var handler = (context.RequestServices.GetService(scheme.HandlerType) ??
        ActivatorUtilities.CreateInstance(context.RequestServices, scheme.HandlerType))
        as IAuthenticationHandler;
    if (handler != null)
    {
        // 根据身份验证方案和HTTPContext初始化身份验证处理程序
        await handler.InitializeAsync(scheme, context);
        // 将身份验证处理程序添加到内部缓存中
        _handlerMap[authenticationScheme] = handler;
    }
    return handler;
}
```

那么`context.RequestServices.GetService(scheme.HandlerType)`获取到的到底是什么身份验证处理程序呢？在前面`StartUp`类的`ConfigureServices`方法中处理配置身份验证，同时还为身份验证配置了Cookie验证。

```C#
services.AddAuthentication(CookieScheme) // Sets the default scheme to cookies
        .AddCookie(CookieScheme, options =>     // 配置Cookie验证
        {
            options.AccessDeniedPath = "/account/denied";
            options.LoginPath = "/account/login";
        });
```

调用的是`CookieExtensions`类中的扩展方法`AddCookie`，源码为：

```C#
/// <summary>
/// 使用指定的方案添加缓存方案验证到<see cref="AuthenticationBuilder"/>.
/// <para>
/// Cookie身份验证使用持久化在客户端中的HTTP Cookie来执行身份验证。
/// </para>
/// </summary>
/// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
/// <param name="authenticationScheme">身份验证方案.</param>
/// <param name="displayName">身份验证处理程序的显示名称.</param>
/// <param name="configureOptions">配置委托 <see cref="CookieAuthenticationOptions"/>.</param>
/// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
public static AuthenticationBuilder AddCookie(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<CookieAuthenticationOptions> configureOptions)
{
    builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>());
    builder.Services.AddOptions<CookieAuthenticationOptions>(authenticationScheme).Validate(o => o.Cookie.Expiration == null, "Cookie.Expiration is ignored, use ExpireTimeSpan instead.");

    // 关键代码 身份验证构造添加方案
    return builder.AddScheme<CookieAuthenticationOptions, CookieAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
}
```

`AuthenticationBuilder`类的`AddScheme`方法实际调用的是该类中的私有方法`AddSchemeHelper`，源码为：

```C#
// TOptions 在这里是CookieAuthenticationOptions（Cookie身份验证选项）
// THandler是CookieAuthenticationHandler（Cookie身份验证处理程序）
private AuthenticationBuilder AddSchemeHelper<TOptions, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]THandler>(string authenticationScheme, string? displayName, Action<TOptions>? configureOptions)
    where TOptions : AuthenticationSchemeOptions, new()
    where THandler : class, IAuthenticationHandler
{
    Services.Configure<AuthenticationOptions>(o =>
    {
        o.AddScheme(authenticationScheme, scheme => {
            scheme.HandlerType = typeof(THandler);
            scheme.DisplayName = displayName;
        });
    });
    if (configureOptions != null)
    {
        Services.Configure(authenticationScheme, configureOptions);
    }
    Services.AddOptions<TOptions>(authenticationScheme).Validate(o => {
        o.Validate(authenticationScheme);
        return true;
    });

    // 配置瞬时生命周期Cookie身份验证处理程序
    Services.AddTransient<THandler>();
    return this;
}
```

所以在该示例中`AuthenticationService`的`SignInAsync`方法实际调用的是[`CookieAuthenticationHandler`](https://sourcegraph.com/github.com/dotnet/aspnetcore/-/blob/src/Security/Authentication/Cookies/src/CookieAuthenticationHandler.cs#L276)从[`SignInAuthenticationHandler`](https://sourcegraph.com/github.com/dotnet/aspnetcore@b0a6755b5ef103b57394f115613b45a37912600a/-/blob/src/Security/Authentication/Core/src/SignInAuthenticationHandler.cs#L15:27)继承的`SignInAsync`方法，该方法的源码为：

```C#
/// <inheritdoc/>
public virtual Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
{
    var target = ResolveTarget(Options.ForwardSignIn);
    return (target != null)
        ? Context.SignInAsync(target, user, properties)
        : HandleSignInAsync(user, properties ?? new AuthenticationProperties());
}
```

这个方法调用的是`HandleSignInAsync`方法，源码为：

```C#
/// <inheritdoc />
protected async override Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
{
    if (user == null)
    {
        throw new ArgumentNullException(nameof(user));
    }

    properties = properties ?? new AuthenticationProperties();

    _signInCalled = true;

    // Process the request cookie to initialize members like _sessionKey.
    await EnsureCookieTicket();
    var cookieOptions = BuildCookieOptions();

    var signInContext = new CookieSigningInContext(
        Context,
        Scheme,
        Options,
        user,
        properties,
        cookieOptions);

    DateTimeOffset issuedUtc;
    if (signInContext.Properties.IssuedUtc.HasValue)
    {
        issuedUtc = signInContext.Properties.IssuedUtc.Value;
    }
    else
    {
        issuedUtc = Clock.UtcNow;
        signInContext.Properties.IssuedUtc = issuedUtc;
    }

    if (!signInContext.Properties.ExpiresUtc.HasValue)
    {
        signInContext.Properties.ExpiresUtc = issuedUtc.Add(Options.ExpireTimeSpan);
    }

    // 触发登陆事件
    await Events.SigningIn(signInContext);

    if (signInContext.Properties.IsPersistent)
    {
        var expiresUtc = signInContext.Properties.ExpiresUtc ?? issuedUtc.Add(Options.ExpireTimeSpan);
        signInContext.CookieOptions.Expires = expiresUtc.ToUniversalTime();
    }

    // 关键代码1 产生身份验证票据
    var ticket = new AuthenticationTicket(signInContext.Principal!, signInContext.Properties, signInContext.Scheme.Name);

    if (Options.SessionStore != null)
    {
        if (_sessionKey != null)
        {
            // Renew the ticket in cases of multiple requests see: https://github.com/dotnet/aspnetcore/issues/22135
            await Options.SessionStore.RenewAsync(_sessionKey, ticket);
        }
        else
        {
            _sessionKey = await Options.SessionStore.StoreAsync(ticket);
        }

        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[] { new Claim(SessionIdClaim, _sessionKey, ClaimValueTypes.String, Options.ClaimsIssuer) },
                Options.ClaimsIssuer));
        ticket = new AuthenticationTicket(principal, null, Scheme.Name);
    }

    // 关键代码2 格式化身份验证票据作为cookie的值
    var cookieValue = Options.TicketDataFormat.Protect(ticket, GetTlsTokenBinding());

    // 关键代码3 使用cookie管理将cookie添加到响应中
    Options.CookieManager.AppendResponseCookie(
        Context,
        Options.Cookie.Name!,
        cookieValue,
        signInContext.CookieOptions);

    var signedInContext = new CookieSignedInContext(
        Context,
        Scheme,
        signInContext.Principal!,
        signInContext.Properties,
        Options);

    // 触发登陆成功事件
    await Events.SignedIn(signedInContext);

    // Only redirect on the login path
    var shouldRedirect = Options.LoginPath.HasValue && OriginalPath == Options.LoginPath;
    await ApplyHeaders(shouldRedirect, signedInContext.Properties);

    // 记录日志
    Logger.AuthenticationSchemeSignedIn(Scheme.Name);
}
```

我们查看关键代码3，看Cookie管理器到底是什么样的。从[微软的文档](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.cookies.cookieauthenticationoptions?view=aspnetcore-3.1)可知[`CookieAuthenticationOptions`](https://sourcegraph.com/github.com/dotnet/aspnetcore/-/blob/src/Security/Authentication/Cookies/src/CookieAuthenticationOptions.cs#L129)默认的cookie管理器为[`ChunkingCookieManager`](https://sourcegraph.com/github.com/dotnet/aspnetcore/-/blob/src/Shared/ChunkingCookieManager/ChunkingCookieManager.cs#L152)，`AppendResponseCookie`方法的源码为：

```C#
/// <summary>
/// 添加一个新的Cookie到 Set-Cookie 响应头. 如果cookie大于给定的大小限制，那么它将被分解为多个cookie，如下所示:
/// Set-Cookie: CookieName=chunks-3; path=/
/// Set-Cookie: CookieNameC1=Segment1; path=/
/// Set-Cookie: CookieNameC2=Segment2; path=/
/// Set-Cookie: CookieNameC3=Segment3; path=/
/// </summary>
/// <param name="context">HTTPContext</param>
/// <param name="key">Cookie Key</param>
/// <param name="value">Cookie Value</param>
/// <param name="options">Cookie 选项</param>
public void AppendResponseCookie(HttpContext context, string key, string? value, CookieOptions options)
{
    if (context == null)
    {
        throw new ArgumentNullException(nameof(context));
    }

    if (key == null)
    {
        throw new ArgumentNullException(nameof(key));
    }

    if (options == null)
    {
        throw new ArgumentNullException(nameof(options));
    }

    var template = new SetCookieHeaderValue(key)
    {
        Domain = options.Domain,
        Expires = options.Expires,
        SameSite = (Net.Http.Headers.SameSiteMode)options.SameSite,
        HttpOnly = options.HttpOnly,
        Path = options.Path,
        Secure = options.Secure,
        MaxAge = options.MaxAge,
    };

    var templateLength = template.ToString().Length;

    value = value ?? string.Empty;

    // Normal cookie
    var responseCookies = context.Response.Cookies;
    if (!ChunkSize.HasValue || ChunkSize.Value > templateLength + value.Length)
    {
        // 将无需拆分的cookie值直接添加到响应的cookie中
        responseCookies.Append(key, value, options);
    }
    else if (ChunkSize.Value < templateLength + 10)
    {
        // 10 is the minimum data we want to put in an individual cookie, including the cookie chunk identifier "CXX".
        // No room for data, we can't chunk the options and name
        throw new InvalidOperationException("The cookie key and options are larger than ChunksSize, leaving no room for data.");
    }
    else
    {
        // 将cookie分解为多个cookie。
        // Key = CookieName, value = "Segment1Segment2Segment2"
        // Set-Cookie: CookieName=chunks-3; path=/
        // Set-Cookie: CookieNameC1="Segment1"; path=/
        // Set-Cookie: CookieNameC2="Segment2"; path=/
        // Set-Cookie: CookieNameC3="Segment3"; path=/
        var dataSizePerCookie = ChunkSize.Value - templateLength - 3; // Budget 3 chars for the chunkid.
        var cookieChunkCount = (int)Math.Ceiling(value.Length * 1.0 / dataSizePerCookie);

        responseCookies.Append(key, ChunkCountPrefix + cookieChunkCount.ToString(CultureInfo.InvariantCulture), options);

        var offset = 0;
        for (var chunkId = 1; chunkId <= cookieChunkCount; chunkId++)
        {
            var remainingLength = value.Length - offset;
            var length = Math.Min(dataSizePerCookie, remainingLength);
            var segment = value.Substring(offset, length);
            offset += length;

            responseCookies.Append(key + ChunkKeySuffix + chunkId.ToString(CultureInfo.InvariantCulture), segment, options);
        }
    }
}
```

获取Cookie的方法源码：

```C#
/// <summary>
/// 得到重新组装的cookie。非块cookie正常返回。
/// Cookies with missing chunks just have their "chunks-XX" header returned.
/// </summary>
/// <param name="context"></param>
/// <param name="key"></param>
/// <returns>The reassembled cookie, if any, or null.</returns>
public string? GetRequestCookie(HttpContext context, string key)
{
    if (context == null)
    {
        throw new ArgumentNullException(nameof(context));
    }

    if (key == null)
    {
        throw new ArgumentNullException(nameof(key));
    }

    // 获取到Cookie的值
    var requestCookies = context.Request.Cookies;
    var value = requestCookies[key];

    // 组装cookie的值
    var chunksCount = ParseChunksCount(value);
    if (chunksCount > 0)
    {
        var chunks = new string[chunksCount];
        for (var chunkId = 1; chunkId <= chunksCount; chunkId++)
        {
            var chunk = requestCookies[key + ChunkKeySuffix + chunkId.ToString(CultureInfo.InvariantCulture)];
            if (string.IsNullOrEmpty(chunk))
            {
                if (ThrowForPartialCookies)
                {
                    var totalSize = 0;
                    for (int i = 0; i < chunkId - 1; i++)
                    {
                        totalSize += chunks[i].Length;
                    }
                    throw new FormatException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            "The chunked cookie is incomplete. Only {0} of the expected {1} chunks were found, totaling {2} characters. A client size limit may have been exceeded.",
                            chunkId - 1,
                            chunksCount,
                            totalSize));
                }
                // Missing chunk, abort by returning the original cookie value. It may have been a false positive?
                return value;
            }

            chunks[chunkId - 1] = chunk;
        }

        return string.Join(string.Empty, chunks);
    }
    return value;
}
```

## 登陆管道总结

未完待续···
