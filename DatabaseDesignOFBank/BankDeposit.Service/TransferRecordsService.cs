using BankDeposit.Data;
using BankDeposit.Model.SqlBank;
using System;
using System.Collections.Generic;
using System.Text;
using BankDeposit.Model.Helper;
namespace BankDeposit.Service
{
    /// <summary>
    /// Records表的业务逻辑处理类。
    /// </summary>
    public class TransferRecordsService
    {
        #region 实例化一些工具对象
        public static AccessTransferRecords acessTransferRecords = new AccessTransferRecords();
        #endregion

        #region 查询某卡上次活期取款，活期存款的交易记录时间。
        /// <summary>
        /// 查询某卡上次活期取款，活期存款的交易记录时间。
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        internal DateTime RecordsTimeData(int cid)
        {
            return (DateTime)acessTransferRecords.TransferRecordsTimeData(cid);
        }
        #endregion

        #region 增加转账记录
        internal void AddtransferRecordsService(Transferrecords transferrecords)
        {
            acessTransferRecords.Add(transferrecords);
        } 
        #endregion

        #region 查询十项转账记录
        /// <summary>
        /// TransferRecordsService层用来查询前十项转账记录的函数
        /// </summary>
        /// <param name="cid">传入从cooike中查询的cid</param>
        /// <returns>返回一个根据储户当前默认银行卡的转账记录，指含有转出或转入的记录，取前十项</returns>
        internal List<Transferrecords> TenTransferRecordsService(int cid)
        {
            return acessTransferRecords.TenTransferRecordsData(cid);
        }
        #endregion

    }
}
