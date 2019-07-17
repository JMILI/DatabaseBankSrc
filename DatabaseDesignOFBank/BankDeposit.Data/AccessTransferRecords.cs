using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankDeposit.Model.SqlBank;
using Microsoft.EntityFrameworkCore;
using BankDeposit.Model.Helper;

namespace BankDeposit.Data
{
    public class AccessTransferRecords
    {
        #region 查询前十项记录
        /// <summary>
        /// 查询最近十项记录,根据cid查询
        /// </summary>
        /// <returns></returns>
        public List<Transferrecords> TenTransferRecordsData(int cid)
        {
            using (bankContext dbContext = new bankContext())
            {
                return dbContext.Transferrecords.FromSql("select * from Transferrecords where TpartyAcid={0} or TpartyBcid={0} order by Tid desc", cid).AsNoTracking().Take(10).ToList();
            }
        }
        #endregion

        #region 增加转账记录

        public void AddData(Transferrecords transferrecord)
        {
            using (var dbContext = new bankContext())
            {
                //修改数据库信息最好有一些事务操作
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        dbContext.Add(transferrecord);
                        dbContext.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        transaction.Rollback();
                    }
                }
            }
        }


        #endregion

        #region 取出最后一次交易记录时间
        /// <summary>
        /// 根据cid取出最后一次交易记录时间
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public DateTime? TransferRecordsTimeData(int cid)
        {
            DateTime t2 = DateTime.MinValue;
            Transferrecords record = new Transferrecords();
            record = null;
            using (bankContext dbContext = new bankContext())
            {
                int cid1 = cid;
                //取出记录表中该卡活动的记录中取涉及转账时账号为cid的第一项记录，
                //记录以降序排列，就可以取出最近对活期存款的操作记录//这里有巴哥
                record = dbContext.Transferrecords.FromSql("select * from Transferrecords where TpartyAcid={0} or TpartyBcid={1}   order by Tid desc", cid, cid1).AsNoTracking().ToList().FirstOrDefault();
                if (record != null) t2 = (DateTime)record.TtransferTime;
            }
            return t2;
        }
        #endregion

    }
}
