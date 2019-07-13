using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankDeposit.Model.SqlBank;
using Microsoft.EntityFrameworkCore;
using BankDeposit.Model.Helper;

namespace BankDeposit.Data
{
    public class AccessGovernors
    {
        #region 查询是否有管理员 （注册的）H
        /// <summary>
        /// 传入的对象为Managers
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public Managers CheakData(int mid)
        {
            Managers governor = new Managers();
            using (var dbContext = new bankContext())
            {
                return governor = dbContext.Managers.FirstOrDefault(a => a.Mid == mid);
            }
        }
        #endregion
        #region 增加柜台管理员
        /// <summary>
        /// 增加柜台管理员
        /// </summary>
        /// <param name="manager">Mid,Mname,MPassword</param>
        public void AddData(Managers manager)
        {
            using (var dbContext = new bankContext())
            {
                //修改数据库信息最好有一些事务操作
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        dbContext.Add(manager);
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

        #region 查询管理员(不含行长）H
        /// <summary>
        /// 查询所有管理员，返回身份为管理员的列表数据
        /// </summary>
        /// <returns>返回身份为管理员的列表数据</returns>
        public List<Managers> QueryManagerData()
        {
            using (var dbContext = new bankContext())
            {
                return dbContext.Managers.FromSql("select * from Managers where Midentify='管理员'").AsNoTracking().ToList();
            }
        }
        #endregion


    }
}
