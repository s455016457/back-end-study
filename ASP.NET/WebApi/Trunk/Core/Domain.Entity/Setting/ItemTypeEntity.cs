using System;
using Common.Extended;

namespace Domain.Entity.Setting
{
    /// <summary>
    /// 物料次类
    /// </summary>
    public class ItemTypeEntity
    {
        private string _code;

        /// <summary>
        /// 次类编号
        /// </summary>
        public String Code
        {
            get { return _code; }
            set
            {
                if (value == null || String.IsNullOrWhiteSpace(value.Trim())) throw new Exception("次类编号不能为空！");
                _code = value.Trim();
            }
        }
        public String ParentID { get; set; }

        #region 其他属性
        private decimal _minScpRate;
        private decimal _maxScpRate;
        private int _itemNumMinLength;
        private int _itemNumMaxLength;
        private string _descr;
        private decimal _overQty;
        private decimal _overRate;
        private string _itemGroup;
        /// <summary>
        /// 次类名称
        /// </summary>
        public String Descr
        {
            get { return _descr; }
            set
            {
                if (value == null || String.IsNullOrWhiteSpace(value.Trim())) throw new Exception("次类名称不能为空！");
                _descr = value.Trim();
            }
        }
        /// <summary>
        /// 物料编码最小长度
        /// </summary>
        public int ItemNumMinLength
        {
            get { return _itemNumMinLength; }
            set
            {
                if (value < 0) throw new Exception("物料编码最小长度不能小于0！");
                _itemNumMinLength = value;
            }
        }
        /// <summary>
        /// 物料编码最大长度
        /// </summary>
        public int ItemNumMaxLength
        {
            get { return _itemNumMaxLength; }
            set
            {
                if (value < ItemNumMinLength) throw new Exception("物料编码最大长度不能小于物料编码最小长度！");
                _itemNumMaxLength = value;
            }
        }
        /// <summary>
        /// 属性类别
        /// </summary>
        public string AttribType { get; set; }
        /// <summary>
        /// 存货科目
        /// </summary>
        public string InvAcc { get; set; }
        /// <summary>
        /// 采购/委外加工收货时可超出合同数量
        /// </summary>
        public Decimal OverQty
        {
            get { return _overQty; }
            set
            {
                if (value < 0) throw new Exception("采购/委外加工收货时可超出合同数量不能小于0！");
                _overQty = value;
            }
        }
        /// <summary>
        /// 生产单及委外加工合同完工数量参考
        /// </summary>
        public Boolean IsFgRef { get; set; }
        /// <summary>
        /// 物料大类
        /// </summary>
        public string ItemGroup
        {
            get { return _itemGroup; }
            set
            {
                if (value == null || String.IsNullOrWhiteSpace(value.Trim())) throw new Exception("物料大类不能为空！");
                _itemGroup = value;
            }
        }
        /// <summary>
        /// 虚拟物料
        /// </summary>
        public Boolean IsVirtual { get; set; }
        /// <summary>
        /// 批次管理
        /// </summary>
        public Boolean IsLotManaged { get; set; }
        /// <summary>
        /// 采购/委外加工收货时可超出合同比率
        /// </summary>
        public Decimal OverRate
        {
            get { return _overRate; }
            set
            {
                if (value < 0) throw new Exception("采购/委外加工收货时可超出合同比率不能小于0！");
                _overRate = value;
            }
        }
        /// <summary>
        /// 物料编码前缀
        /// </summary>
        public String PreFix { get; set; }
        /// <summary>
        /// 最小损耗率
        /// </summary>
        public Decimal MinScpRate
        {
            get { return _minScpRate; }
            set
            {
                if (value < 0) throw new Exception("最小损耗率不能小于0！");
                _minScpRate = value;
            }
        }
        /// <summary>
        /// 最大损耗率
        /// </summary>
        public Decimal MaxScpRate
        {
            get { return _maxScpRate; }
            set
            {
                if (value < MinScpRate) throw new Exception("最大损耗率不能小于最小损耗率！");
                _maxScpRate = value;
            }
        }
        #endregion

        /// <summary>
        /// 获取或设置实体的状态。
        /// </summary>
        public EntityState State { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return this.ValueEquest(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
