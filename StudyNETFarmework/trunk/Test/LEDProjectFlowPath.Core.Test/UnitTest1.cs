using System;
using System.Collections.Generic;
using LEDProjectFlowPath.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Linq.Expressions;
using LEDProjectFlowPath.Core.Interface;

namespace LEDProjectFlowPath.Core.Test
{
    [TestClass]
    public class UnitTest1
    {
        List<IDataReport> dataReports = new List<IDataReport>();
        List<IReviewReport> reviewReports = new List<IReviewReport>();
        List<IWorkFlow> workFlows = new List<IWorkFlow>();
        IProjectFlow projectFlow = null;


        public UnitTest1()
        {
            var 立项申请 = new DataReport(Guid.Parse("3C50B6B4-5F15-4F76-A63C-1C3505286F61"), "立项申请", "表单"
                , "立项申请预览视图", "立项申请主界面", DataReportStatus.UnCommit);
            var 项目开发通知书 = new DataReport(Guid.Parse("0824D4E9-2508-40EE-AE3D-B554DF8531AF"), "项目开发通知书", "表单"
                , "项目开发通知书预览视图", "项目开发通知书主界面", DataReportStatus.UnCommit);
            var 开发任务书 = new DataReport(Guid.Parse("B9EC8360-1A27-41C0-98FA-97AD4B9E11FA"), "开发任务书", "表单"
                , "开发任务书预览视图", "开发任务书主界面", DataReportStatus.UnCommit);
            var 草案设计 = new DataReport(Guid.Parse("DB3964BA-C8B0-420E-BA66-CD202D673EAF"), "草案设计", "表单"
                , "草案设计预览视图", "草案设计主界面", DataReportStatus.UnCommit);
            var 寿命模拟 = new DataReport(Guid.Parse("6205156A-082B-4B8E-B117-659DD052C275"), "寿命模拟", "表单"
                , "寿命模拟预览视图", "寿命模拟主界面", DataReportStatus.UnCommit);
            var 测试任务 = new DataReport(Guid.Parse("FDF72E38-903B-46DE-A7EA-A54A9E54E036"), "测试任务", "表单"
                , "测试任务预览视图", "测试任务主界面", DataReportStatus.UnCommit);
            var 外观确认报表 = new DataReport(Guid.Parse("45F89F96-84C5-4F4B-99C0-6E52D6DDD877"), "外观确认报表", "表单"
                , "外观确认报表预览视图", "外观确认报表主界面", DataReportStatus.UnCommit);
            var 光电参数报告 = new DataReport(Guid.Parse("136005C9-ECAF-4A35-9D75-9DCDA67DD7C4"), "光电参数报告", "表单"
                , "光电参数报告预览视图", "光电参数报告主界面", DataReportStatus.UnCommit);
            var T25温升测试报告 = new DataReport(Guid.Parse("019E4970-B7A2-4E8F-8EE0-009B1CA0B78B"), "T25温升测试报告", "表单"
                , "T25温升测试报告预览视图", "T25温升测试报告主界面", DataReportStatus.UnCommit);
            var TIS温度测试报告 = new DataReport(Guid.Parse("DBC04FF4-74EC-4425-9897-77F811819E8D"), "TIS温度测试报告", "表单"
                , "TIS温度测试报告预览视图", "TIS温度测试报告主界面", DataReportStatus.UnCommit);
            var H100稳态测试报告 = new DataReport(Guid.Parse("3835C5E2-5BFC-4FC9-8AEC-0CC2EC5B4193"), "H100稳态测试报告", "表单"
                , "H100稳态测试报告预览视图", "H100稳态测试报告主界面", DataReportStatus.UnCommit);
            var 样品破坏新能测试报告 = new DataReport(Guid.Parse("7DCB762F-A1D7-4AAD-BCFA-C41A89DED261"), "样品破坏新能测试报告", "表单"
                , "样品破坏新能测试报告预览视图", "样品破坏新能测试报告主界面", DataReportStatus.UnCommit);
            var T40W6光衰测试报告 = new DataReport(Guid.Parse("5D106D28-CDDC-4091-816B-67F356B2CF66"), "T40W6光衰测试报告", "表单"
                , "T40W6光衰测试报告预览视图", "T40W6光衰测试报告主界面", DataReportStatus.UnCommit);
            var T40W12光衰测试报告 = new DataReport(Guid.Parse("3EF548AF-AF6E-411E-A4C5-15ED6DDE1416"), "T40W12光衰测试报告", "表单"
                , "T40W12光衰测试报告预览视图", "T40W12光衰测试报告主界面", DataReportStatus.UnCommit);
            var 商品命名 = new DataReport(Guid.Parse("541332A9-6C2E-46C6-AA61-B899FF95BCC2"), "商品命名", "表单"
                , "商品命名预览视图", "商品命名主界面", DataReportStatus.UnCommit);
            var 物料清单编制 = new DataReport(Guid.Parse("2A90394C-E1D6-4177-B196-DB88671F131A"), "物料清单编制", "表单"
                , "物料清单编制预览视图", "物料清单编制主界面", DataReportStatus.UnCommit);
            var 零部件图片上传 = new DataReport(Guid.Parse("B3BD4021-738A-45E2-A910-B6248AF174DF"), "零部件图片上传", "表单"
                , "零部件图片上传预览视图", "零部件图片上传主界面", DataReportStatus.UnCommit);
            var 来料及成品检验上传 = new DataReport(Guid.Parse("BF5D0D23-D2E2-4BA9-B251-8F067F4193EB"), "来料及成品检验上传", "表单"
                , "来料及成品检验上传预览视图", "来料及成品检验上传主界面", DataReportStatus.UnCommit);
            var 销售资料上传 = new DataReport(Guid.Parse("9030CC6A-DE11-4994-9F69-9E039CA7FBFB"), "销售资料上传", "表单"
                , "销售资料上传预览视图", "销售资料上传主界面", DataReportStatus.UnCommit);
            var 产品规格书 = new DataReport(Guid.Parse("113901D3-AFF4-430F-A1A3-F5C40515FA24"), "产品规格书", "表单"
                , "产品规格书预览视图", "产品规格书主界面", DataReportStatus.UnCommit);


            dataReports.AddRange(new DataReport[] {
                立项申请,
                项目开发通知书,
                开发任务书,
                草案设计,
                寿命模拟,
                测试任务,
                外观确认报表,
                光电参数报告,
                T25温升测试报告,
                TIS温度测试报告,
                H100稳态测试报告,
                样品破坏新能测试报告,
                T40W6光衰测试报告,
                T40W12光衰测试报告,
                商品命名,
                物料清单编制,
                零部件图片上传,
                来料及成品检验上传,
                销售资料上传,
                产品规格书,
            });

            var 项目可行性论证报告 = new ReviewReport(Guid.Parse("EC3F742D-AFA6-420C-9180-2785BA0A823A"), "项目可行性论证报告", "评审"
                , "项目可行性论证报告预览视图", "项目可行性论证报告主界面", DataReportType.Serial, ReviewReportStatus.Pending, false,
                dataReports.Where(p => p.Name.Equals("立项申请")).ToList());

            var 技术方案论证报告 = new ReviewReport(Guid.Parse("90EC9D9C-7C70-4650-87B3-5A6319A56452"), "技术方案论证报告", "评审"
                , "技术方案论证报告预览视图", "技术方案论证报告主界面", DataReportType.Serial, ReviewReportStatus.Pending, false,
                dataReports.Where(p => p.Name.Equals("寿命模拟")).ToList());

            var 产品性能及定性评审 = new ReviewReport(Guid.Parse("86B8FCD0-DA9E-41FD-AAA8-C8E9F16DE73A"), "产品性能及定性评审", "评审"
                , "产品性能及定性评审预览视图", "产品性能及定性评审主界面", DataReportType.Parallel, ReviewReportStatus.Pending, true,
                dataReports.Where(p => p.Name.Equals("外观确认报表") || p.Name.Equals("光电参数报告")
                || p.Name.Equals("T25温升测试报告") || p.Name.Equals("TIS温度测试报告")).ToList());

            var 一阶段性能评审 = new ReviewReport(Guid.Parse("90EC9D9C-7C70-4650-87B3-5A6319A56452"), "一阶段性能评审", "评审"
                , "一阶段性能评审预览视图", "一阶段性能评审主界面", DataReportType.Parallel, ReviewReportStatus.Pending, false,
                dataReports.Where(p => p.Name.Equals("H100稳态测试报告") || p.Name.Equals("样品破坏新能测试报告")).ToList());

            var 二阶段性能评审 = new ReviewReport(Guid.Parse("6DCE7736-70A2-4848-ACF7-388F2F49732C"), "二阶段性能评审", "评审"
                , "二阶段性能评审预览视图", "二阶段性能评审主界面", DataReportType.Parallel, ReviewReportStatus.Pending, false,
                dataReports.Where(p => p.Name.Equals("T40W6光衰测试报告")).ToList());

            var 综合性能评审 = new ReviewReport(Guid.Parse("11C27E8F-C3B5-4F6F-8915-DF31C2AA13D7"), "综合性能评审", "评审"
                , "综合性能评审预览视图", "综合性能评审主界面", DataReportType.Parallel, ReviewReportStatus.Pending, false,
                dataReports.Where(p => p.Name.Equals("T40W12光衰测试报告")).ToList());

            var 资料评审= new ReviewReport(Guid.Parse("FAD8FA27-3F9B-4643-9B09-271B476296CE"), "资料评审", "评审"
                , "资料评审预览视图", "资料评审主界面", DataReportType.Parallel, ReviewReportStatus.Pending, false,
                dataReports.Where(p => p.Name.Equals("零部件图片上传")
                    ||p.Name.Equals("来料及成品检验上传") 
                    || p.Name.Equals("销售资料上传")
                    || p.Name.Equals("产品规格书"))
                .ToList());

            reviewReports.AddRange(new ReviewReport[] { 项目可行性论证报告,
                技术方案论证报告,
                产品性能及定性评审,
                一阶段性能评审,
                二阶段性能评审 ,
                综合性能评审,
                资料评审
            });
            
            var 决策 = new WorkFlow(Guid.Parse("5EAE9517-12AC-474D-81AB-401B78A24101")
                , "决策",Guid.Parse("0A9DEBFE-CFEF-43A5-9C21-E263BA682126")
                ,new List<IWorkTask> { 立项申请 , 项目可行性论证报告 });
            var 设计前准备 = new WorkFlow(Guid.Parse("DC65EFB1-D802-4E5E-951D-3F340044608D")
                , "设计前准备", Guid.Parse("A963D7E8-3A1C-4192-8914-D36950AF5043")
                ,new List<IWorkTask> { 项目开发通知书, 开发任务书 });
            var 方案设计 = new WorkFlow(Guid.Parse("9A3827EA-0A70-492C-A27F-5DAAEC74256C")
                , "方案设计", Guid.Parse("41467A9D-331D-4815-81F8-68492CBF9057")
                ,new List<IWorkTask> { 草案设计, 技术方案论证报告 });
            var 方案验证 = new WorkFlow(Guid.Parse("4B28A1C5-150D-4C4D-AB60-6967BBEE025C")
                , "方案验证", Guid.Parse("CCFE6FE2-D7BF-4D25-9E86-77328E12B6C4")
                ,new List<IWorkTask> { 测试任务, 产品性能及定性评审, 一阶段性能评审, 二阶段性能评审, 综合性能评审 });
            var 资料设计 = new WorkFlow(Guid.Parse("71D09EFC-F2A7-4C9D-AC7A-AC097704F8C1")
                , "资料设计", Guid.Parse("F1465D41-AD74-4AE4-89DC-A3E8FA656DE3")
                ,new List<IWorkTask> { 商品命名, 资料评审 });

            projectFlow = new ProjectFlow(Guid.Parse("C0A00706-E619-4F71-A8A2-77D3F7478F62"), "项目管理流程"
                , Guid.Parse("A19300A7-8D7F-4571-895B-A0DD1B035ED7"), new List<IWorkFlow>
            {
                设计前准备,方案设计,方案验证,资料设计
            });
        }

        [TestMethod]
        public void TestMethod1()
        {
            projectFlow.OnWorkTaskStatusChange += ProjectFlow_OnWorkTaskStatusChange;
            projectFlow.IFlows.FirstOrDefault().UpdateWorkTaskStatus(WorkTaskStatus.Executing);
            try
            {
                dataReports.FirstOrDefault(p => p.Name.Equals("项目开发通知书")).UpdateDataReportStatus(DataReportStatus.Commit);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "工作任务处于等待状态，不能修改数据报告状态！");   
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            projectFlow.OnWorkTaskStatusChange += ProjectFlow_OnWorkTaskStatusChange;
            projectFlow.IFlows.FirstOrDefault().UpdateWorkTaskStatus(WorkTaskStatus.Executing);
            try
            {
                projectFlow.ExecutNextWorkTask();
                dataReports.FirstOrDefault(p => p.Name.Equals("项目开发通知书")).UpdateDataReportStatus(DataReportStatus.Commit);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "工作任务处于等待状态，不能修改数据报告状态！");
            }
        }

        private void ProjectFlow_OnWorkTaskStatusChange(IWorkTask workTask)
        {
            Console.WriteLine("");
        }
    }
}
