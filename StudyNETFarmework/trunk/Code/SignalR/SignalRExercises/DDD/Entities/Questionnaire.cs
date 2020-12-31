using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SignalRExercises.DDD.Entities
{
    /// <summary>
    /// 问卷
    /// </summary>
    public class Questionnaire
    {
        /// <summary>
        /// 问卷ID
        /// </summary>
        public Guid ID { get; private set; }
        /// <summary>
        /// 问卷题目
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// 问题难度
        /// </summary>
        public QuestionDifficulty QuestionDifficulty { get; private set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedStamp { get; private set; }
        /// <summary>
        /// 问卷试题
        /// </summary>
        public virtual List<QuestionnaireQuestion> QuestionnaireQuestions { get; private set; }

        private Questionnaire() { }
        /// <summary>
        /// 创建问卷
        /// </summary>
        /// <param name="title"></param>
        /// <param name="questionDifficulty"></param>
        /// <returns></returns>
        public static Questionnaire CreateNew(string title, QuestionDifficulty questionDifficulty, IEnumerable<Question> questions)
        {
            var model  = new Questionnaire
            {
                ID = Guid.NewGuid(),
                Title = title,
                QuestionDifficulty = questionDifficulty,
                CreatedStamp = DateTime.Now,
                QuestionnaireQuestions = new List<QuestionnaireQuestion>()
            };
            foreach(var question in questions)
            {
                model.QuestionnaireQuestions.Add(QuestionnaireQuestion.CreateNew(model, question));
            }
            return model;
        }

        /// <summary>
        /// 添加问题
        /// </summary>
        /// <param name="questions"></param>
        public void AddQuestionnaireQuestions(params Question[] questions)
        {
            foreach (var question in questions)
            {
                QuestionnaireQuestions.Add(QuestionnaireQuestion.CreateNew(this, question));
            }
        }

        /// <summary>
        /// 添加问题
        /// </summary>
        public void AddQuestionnaireQuestions(IEnumerable<Question> questions)
        {
            foreach (var question in questions)
            {
                QuestionnaireQuestions.Add(QuestionnaireQuestion.CreateNew(this, question));
            }
        }
    }
    /// <summary>
    /// 问卷题目
    /// </summary>
    public class QuestionnaireQuestion
    {
        /// <summary>
        /// 问卷题目ID
        /// </summary>
        public Guid ID { get; private set; }
        /// <summary>
        /// 问卷ID
        /// </summary>
        public Guid QuestionnaireId { get; private set; }
        /// <summary>
        /// 题目ID
        /// </summary>
        public Guid QuestionId { get; private set; }
        /// <summary>
        /// 问卷
        /// </summary>
        [ForeignKey("QuestionnaireId")]
        public virtual Questionnaire Questionnaire { get; private set; }
        /// <summary>
        /// 题目
        /// </summary>
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; private set; }

        private QuestionnaireQuestion() { }
        /// <summary>
        /// 创建新的问卷题目
        /// </summary>
        /// <param name="questionnaire"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public static QuestionnaireQuestion CreateNew(Questionnaire questionnaire , Question question)
        {
            if (question.QuestionDifficulty != questionnaire.QuestionDifficulty)
                throw new Exception($"问题难度与问卷难度不一致！");

            return new QuestionnaireQuestion
            {
                ID = Guid.NewGuid(),
                QuestionId=question.QuestionGuid,
                Question=question,
                QuestionnaireId=questionnaire.ID,
                Questionnaire=questionnaire
            };
        }
    }
    /// <summary>
    /// 问题
    /// </summary>
    public class Question
    {
        /// <summary>
        /// 问题ID
        /// </summary>
        [Key]
        [Column("ID")]
        public Guid QuestionGuid { get; private set; }
        /// <summary>
        /// 题目
        /// </summary>
        [Column("Title")]
        public string Title { get; private set; }
        /// <summary>
        /// 问题难度
        /// </summary>
        [Column("Difficulty")]
        public QuestionDifficulty QuestionDifficulty { get; private set; }
        /// <summary>
        /// 正确选项
        /// </summary>
        [Column("CorrectOptionId")]
        public Guid CorrectOptionId { get; private set; }
        /// <summary>
        /// 问题选项
        /// </summary>
        [ForeignKey("QuestionGuid")]
        public virtual List<QuestionOption> QuestionOptions { get; private set; }
        /// <summary>
        /// 正确选项
        /// </summary>
        [ForeignKey("CorrectOptionId")]
        public virtual QuestionOption CorrectOption { get; private set; }

        private Question() { }
        /// <summary>
        /// 创建新的问题
        /// </summary>
        /// <param name="title"></param>
        /// <param name="questionDifficulty"></param>
        /// <returns></returns>
        public static Question CreateNew(string title, QuestionDifficulty questionDifficulty)
        {
            return new Question
            {
                QuestionGuid = Guid.NewGuid(),
                Title = title,
                QuestionDifficulty = questionDifficulty,
                CorrectOption = null,
                CorrectOptionId = default(Guid),
                QuestionOptions = new List<QuestionOption>()
            };
        }
        public void UpdateTitle(string title)
        {
            Title = title;
        }
        public void UpdateQuestionDifficulty(QuestionDifficulty questionDifficulty)
        {
            QuestionDifficulty = questionDifficulty;
        }
        /// <summary>
        /// 添加问题
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public QuestionOption AddQuestionOption(string description)
        {
            var questionOption = QuestionOption.CreateNew(this, description);
            if (QuestionOptions == null)
                QuestionOptions = new List<QuestionOption>();
            QuestionOptions.Add(questionOption);
            return questionOption;
        }
        /// <summary>
        /// 修改问题选项
        /// </summary>
        /// <param name="questionOptionGuid"></param>
        /// <param name="description"></param>
        public void UpdateQuestionOption(Guid questionOptionGuid, string description)
        {
            if (QuestionOptions == null)
                throw new Exception($"问题选项{questionOptionGuid}不存在！");
            var qOption = QuestionOptions.FirstOrDefault(p => p.QuestionOptionGuid.Equals(questionOptionGuid));
            if (qOption == null)
                throw new Exception($"问题选项{questionOptionGuid}不存在！");

            qOption.UpdateDescription(description);

            if (qOption.QuestionOptionGuid.Equals(CorrectOptionId))
                CorrectOption = qOption;
        }
        /// <summary>
        /// 删除问题选项
        /// </summary>
        /// <param name="questionOptionGuid"></param>
        public void RemoveQuestionOption(Guid questionOptionGuid)
        {
            if (QuestionOptions == null)
                throw new Exception($"问题选项{questionOptionGuid}不存在！");
            var qOption = QuestionOptions.FirstOrDefault(p => p.QuestionOptionGuid.Equals(questionOptionGuid));
            if (qOption == null)
                throw new Exception($"问题选项{questionOptionGuid}不存在！");

            QuestionOptions.Remove(qOption);
            if (qOption.QuestionOptionGuid.Equals(CorrectOptionId))
            {
                CorrectOptionId = default(Guid);
                CorrectOption = null;
            }
        }
        /// <summary>
        /// 设置正确选项
        /// </summary>
        /// <param name="questionOptionGuid"></param>
        public void SetCorrectOption(Guid questionOptionGuid)
        {
            if (QuestionOptions == null)
                throw new Exception($"问题选项{questionOptionGuid}不存在！");

            var qOption = QuestionOptions.FirstOrDefault(p => p.QuestionOptionGuid.Equals(questionOptionGuid));
            CorrectOption = qOption ?? throw new Exception($"问题选项{questionOptionGuid}不存在！");
            CorrectOptionId = questionOptionGuid;
        }
        /// <summary>
        /// 有真确答案
        /// </summary>
        /// <returns></returns>
        public bool HaveCorrectOption()
        {
            return CorrectOptionId != default(Guid);
        }
    }

    /// <summary>
    /// 问题选项
    /// </summary>
    [Table("QuestionOption")]
    public class QuestionOption
    {
        /// <summary>
        /// 问题选项ID
        /// </summary>
        [Key]
        [Column("ID")]
        public Guid QuestionOptionGuid { get; private set; }
        /// <summary>
        /// 问题ID
        /// </summary>
        [Column("QID")]
        public Guid QuestionGuid { get; private set; }
        /// <summary>
        /// 选项描述
        /// </summary>
        [Column("Description")]
        public string Description { get; private set; }
        /// <summary>
        /// 问题
        /// </summary>
        [ForeignKey("QuestionGuid")]
        public virtual Question Question{get; private set;}

        private QuestionOption() { }
        /// <summary>
        /// 创建新的选项
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static QuestionOption CreateNew(Question question, string description)
        {
            return new QuestionOption
            {
                QuestionOptionGuid = Guid.NewGuid(),
                QuestionGuid= question.QuestionGuid,
                Question=question,
                Description = description
            };
        }

        public void UpdateDescription(string description)
        {
            this.Description = description;
        }
    }

    public enum QuestionDifficulty
    {
        /// <summary>
        /// 初级难度   
        /// </summary>
        Easy = 0,
        /// <summary>
        /// 一般难度
        /// </summary>
        Normal=1,
        /// <summary>
        /// 高级难度
        /// </summary>
        Advanced=2,
        /// <summary>
        /// 精英难度  
        /// </summary>
        Elite = 3,
        /// <summary>
        /// 噩梦难度 
        /// </summary>
        Conqueror = 4,
        /// <summary>
        /// 地狱难度
        /// </summary>
        Guardian =5,
    }
}