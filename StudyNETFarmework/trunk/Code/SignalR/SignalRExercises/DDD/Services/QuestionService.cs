using SignalRExercises.DDD.Entities;
using SignalRExercises.DDD.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SignalRExercises.DDD.Services
{
    public class QuestionService: IService
    {
        ISysRepository repository;

        /// <summary>
        /// 创建问题
        /// </summary>
        /// <param name="title"></param>
        /// <param name="questionDifficulty"></param>
        /// <returns></returns>
        public async Task<Question> CreateQuestionAsync(string title, QuestionDifficulty questionDifficulty)
        {
            var model = Question.CreateNew(title, questionDifficulty);
            repository.Questions.Add(model);
            await repository.SaveChangesAsync();
            return model;
        }
        /// <summary>
        /// 修改问题
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="title"></param>
        /// <param name="questionDifficulty"></param>
        /// <returns></returns>
        public async Task UpdateQuestionAsync(Guid questionId, string title, QuestionDifficulty questionDifficulty)
        {
            var question = repository.Questions.FirstOrDefault(p => p.QuestionGuid.Equals(questionId));
            if (question == null)
                throw new Exception($"问题{questionId}不存在！");

            question.UpdateTitle(title);
            question.UpdateQuestionDifficulty(questionDifficulty);
            await repository.SaveChangesAsync();
        }
        /// <summary>
        /// 添加问题选项
        /// </summary>
        /// <param name="questionId"><问题ID/param>
        /// <param name="descriptions">选项</param>
        /// <returns></returns>
        public async Task<IEnumerable<QuestionOption>> AddQuestionOptionsAsync(Guid questionId,params string[] descriptions)
        {
            var question = repository.Questions.FirstOrDefault(p => p.QuestionGuid.Equals(questionId));
            if (question == null)
                throw new Exception($"问题{questionId}不存在！");
            List<QuestionOption> QuestionOptions = new List<QuestionOption>();
            foreach (var description in descriptions)
            {
                QuestionOptions.Add(question.AddQuestionOption(description));
            }
            await repository.SaveChangesAsync();
            return QuestionOptions;
        }
        /// <summary>
        /// 更新问题选项
        /// </summary>
        /// <param name="QuestionOptionGuid"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task UpdateQuestionOptionAsync(Guid questionId, Guid QuestionOptionGuid,string description)
        {
            var question = repository.Questions.FirstOrDefault(p => p.QuestionGuid.Equals(questionId));
            if (question == null)
                throw new Exception($"问题{questionId}不存在！");

            question.UpdateQuestionOption(QuestionOptionGuid, description);
            await repository.SaveChangesAsync();
        }
        /// <summary>
        /// 删除问题选项
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="QuestionOptionGuid"></param>
        /// <returns></returns>
        public async Task RemoveQuestionOptionAsync(Guid questionId, Guid QuestionOptionGuid)
        {
            var question = repository.Questions.FirstOrDefault(p => p.QuestionGuid.Equals(questionId));
            if (question == null)
                throw new Exception($"问题{questionId}不存在！");

            question.RemoveQuestionOption(QuestionOptionGuid);
            await repository.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (repository != null)
                repository.Dispose();
        }
    }
}