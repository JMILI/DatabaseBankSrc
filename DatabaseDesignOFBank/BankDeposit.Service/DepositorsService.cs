using BankDeposit.Data;
using BankDeposit.Model.SqlBank;
//using Microsoft.AspNetCore.Mvc;
using System.Web.Extensions;
using BankDeposit.Model.Helper;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System;

namespace BankDeposit.Service
{
    public class DepositorsService
    {
        #region 实例化一些工具对象
        public static AccessDepositors accessDepositors = new AccessDepositors();
        public static CardsService cardsService = new CardsService();
        public static DepositorsService depositorsService = new DepositorsService();
        public static FixbalanceService fixbalanceService = new FixbalanceService();
        public static TransferRecordsService transferRecordsService = new TransferRecordsService();
        public static IdcardService idcardService = new IdcardService();
        #endregion

        #region 查询储户 登录
        /// <summary>
        /// 根据封装登录信息的对象User（卡或储户）
        /// </summary>
        /// <param name="user">Id，Password，Identify</param>
        /// <returns>Depositors</returns>
        public DepositorAndCard QueryDepositorsService(User user)
        {
            Depositors depositor = new Depositors();//接受返回值
            DepositorAndCard depositors = new DepositorAndCard();//函数将要返回的对象
            depositor = accessDepositors.QueryDepositorsData(user);
            if (depositor != null)
            {
                if (depositor.Uid != user.Id)
                {
                    depositors = null;
                }
                else
                {
                    depositors.Duid = depositor.Uid;
                    depositors.Dcid = depositor.Ucid;
                    if (depositor.Ucid == null)
                    {
                        depositors.Dname = "您还没有绑定银行卡";
                    }
                    else
                    {
                        depositors.Dname = idcardService.QueryData(depositor.Uicid).Icname;
                    }
                }
            }
            else
            {
                depositors = null;
            }
            return depositors;
        }
        #endregion

        #region 增加储户
        /// <summary>
        /// 判断是否增加储户成功
        /// </summary>
        /// <param name="depositor">表单填写的储户信息Uid,Uname,UPassword</param>
        /// <returns>DepositorAndCard</returns>
        public DepositorAndCard AddService(Depositors depositor)
        {
            DepositorAndCard depositors = new DepositorAndCard();
            Depositors depositor1 = new Depositors();
            depositor1 = accessDepositors.CheakData(depositor.Uid);//查询有没有该储户
            if (depositor1 != null)
            {
                depositors = null;
            }
            else
            {
                accessDepositors.AddData(depositor);
                depositors.Duid = depositor.Uid;
                depositors.Dname = "请绑定银行卡";
            }
            return depositors;
        }


        #endregion

        #region 查询十项交易记录
        /// <summary>
        /// DepositorsService层用来查询前十项交易记录的函数,向CardsService对象发送请求。
        /// </summary>
        /// <param name="cid">传入从cooike中查询的cid</param>
        /// <returns>返回一个根据储户当前默认银行卡的交易记录，取前十项</returns>
        public List<Records> TenRecordsService(int cid)
        {
            return cardsService.TenRecordsService(cid);
        }
        #endregion

        #region 查询十项转账记录
        /// <summary>
        /// DepositorsService层用来查询前十项转账记录的函数,向transferRecordsService对象发送请求。
        /// </summary>
        /// <param name="cid">传入从cooike中查询的cid</param>
        /// <returns>返回一个根据储户当前默认银行卡的转账记录，指含有转出或转入的记录，取前十项</returns>
        public List<Transferrecords> TenTransferRecordsService(int cid)
        {
            return transferRecordsService.TenTransferRecordsService(cid);
        }
        #endregion

        #region 储户绑定卡号
        /// <summary>
        /// 储户绑定卡号,先找到该储户，然后找到该储户要绑定的银行卡，最后绑定
        /// </summary>
        /// <param name="depositor">前端传入DepositorAndCard对象，属性：Duid,Dname,Dcid</param>
        /// <returns></returns>
        public DepositorAndCard UpdataBandService(DepositorAndCard depositor)
        {
            Idcard idcard = new Idcard();
            DepositorAndCard DAndC = new DepositorAndCard();//返回对象

            Cards card = new Cards();//接受返回数据
            Depositors depositor1 = new Depositors();//接受查询到的数据
            depositor1 = accessDepositors.CheakData(depositor.Duid);//查询有没有该储户
            if (depositor1 != null)//有数据
            {
                if (depositor1.Uid == depositor.Duid)//判断查找到的储户是不是我们要找的
                {
                    card = cardsService.CheakCards((int)depositor.Dcid);
                    if (card != null && card.Cuid == depositor.Duid && card.Cicid == depositor.Dicid)//判断查找到的银行卡是不是我们要找的
                    {
                        accessDepositors.UpdataBandData(depositor);//更新绑定卡号
                        idcard = idcardService.QueryData(card.Cicid);
                        DAndC.Dname = idcard.Icname;
                        DAndC.Dcid = depositor.Dcid;
                        DAndC.Duid = depositor.Duid;
                    }
                    else DAndC = null;
                }
                else DAndC = null;
            }
            else DAndC = null;
            return DAndC;
        }

        #endregion

        #region 查询活期存款余额情况
        /// <summary>
        ///  查询活期存款余额情况,是DepositoryService的方法，将查询余额利息的职责交给CardsService类来处理
        /// </summary>
        /// <param name="cid">传入查询的cid</param>
        /// <returns></returns>
        public List<double> FlowBalanceService(int cid)
        {
            return cardsService.FlowBalanceService(cid);
        }
        #endregion

        #region 查询定期余额情况
        /// <summary>
        /// 查询定期余额情况,是DepositoryService的方法，将查询余额利息的职责交给FixbalanceService类来处理
        /// </summary>
        /// <param name="cid">传入查询的cid</param>
        /// <returns></returns>
        public List<Fixbalances> FixBalanceService(int cid)
        {
            return fixbalanceService.FixBalancesService(cid);
        }

        #endregion

        #region 转账
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transferrecords">包含转账的乙方账号，转账金额,转账甲方账号，姓名</param>
        /// <param name="card">密码</param>
        /// <param name="v">传入1，代表储户操作</param>
        /// <returns></returns>
        public bool Transfer(Transferrecords transferrecords, string password)
        {
            bool boo = false;
            Cards card1 = new Cards();
            Cards card2 = new Cards();
            Depositors depositor = new Depositors();
            //找对方账号，姓名,
            card1 = cardsService.CheakCards(transferrecords.TpartyBcid);
            if (card1 != null && card1.Cid == transferrecords.TpartyBcid)//找对方银行卡号
            { //找到则找自己账号。钱够不够
                card2 = cardsService.CheakCards(transferrecords.TpartyAcid);

                if (card2 != null && card2.Cid == transferrecords.TpartyAcid && card2.Cpassword == password) //找甲方（自己）银行卡号并验证密码
                {
                    if (card2.CflowBalance >= transferrecords.TtransferBalance)
                    {
                        //够则修改自己的账户余额（减少）
                        card2.CflowBalance = (double)(card2.CflowBalance - transferrecords.TtransferBalance);
                        cardsService.UpdataFlowBalanceService(card2);
                        //给对方增加钱
                        card1.CflowBalance = (double)(card1.CflowBalance + transferrecords.TtransferBalance);
                        cardsService.UpdataFlowBalanceService(card1);
                        //生成转账记录
                        transferrecords.TpartyBname = idcardService.QueryData(card1.Cicid).Icname;//找到名字并加入
                        transferRecordsService.AddtransferRecordsService(transferrecords);
                        boo = true;
                    }
                }
            }
            return boo;
        }
        #endregion
    }
}
