namespace Domain.ValueObject
{
    public enum PoExecStatus
    {
        /// <summary>
        /// 未执行
        /// </summary>
        UnExce = 0,
        /// <summary>
        /// 已通知
        /// </summary>
        Notified = 1,
        /// <summary>
        /// 已收货
        /// </summary>
        Grned = 2,
        /// <summary>
        /// 已退货
        /// </summary>
        Pured = 3,
        /// <summary>
        /// 已结案
        /// </summary>
        Closed = 9
    }
}
