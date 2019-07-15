using BankDeposit.Data;
using BankDeposit.Model.SqlBank;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using BankDeposit.Model.Helper;
using System.Linq;
using System.Text;

namespace BankDeposit.Service
{
    public class CardsService
    {
        #region 实例化一些工具对象
        public static AccessCards CardsAccess = new AccessCards();
        public static AccessDepositors DepositorsAccess = new AccessDepositors();
        public static RecordsService recordsService = new RecordsService();
        public static CardsService cardServive = new CardsService();
        public static TransferRecordsService transferRecordsService = new TransferRecordsService();
        public static IdcardService idcardService = new IdcardService();

        #endregion

        #region 查询卡登录
        /// <summary>
        /// 查询卡通过传入的User查询。User：Id,Password,Identify
        /// </summary>
        /// <param name="user">传入登录信息，账号Id,Password,Identify</param>
        /// <returns></returns>
        public DepositorAndCard QueryCardsService(User user)
        {
            DepositorAndCard cards = new DepositorAndCard();//函数将要返回的对象
            Cards card = new Cards();//接受查询结果数据
            Depositors depositor = new Depositors();//接受查询结果数据
            card = CardsAccess.QueryCardsData(user);
            if (card != null)//查询不空
            {
                if (card.Cid != user.Id)//不是要查询的对象
                {
                    cards = null;
                }
                else
                {
                    cards.Duid = (int)card.Cuid;//返回对象装入值
                    cards.Dcid = card.Cid;
                    cards.Dname = idcardService.QueryService(card.Cicid).Icname;
                }
            }
            else
            {
                cards = null;
            }
            return cards;
        }
        #endregion

        #region 取钱
        /// <summary>
        /// ATM活期取款功能
        /// </summary>
        /// <param name="dAndC"></param>
        /// <param name="identity">传入操作类型，此处为1，为取款</param>
        /// <param name="money">传入取钱金额</param>
        /// <returns></returns>
        public bool DrawalService(DepositorAndCard dAndC, int identity, double money)
        {
            List<Double> record = new List<Double>();
            record = FlowBalanceService((int)dAndC.Dcid);//查询银行卡活期的余额
            if (record[1] >= money)//可取
            {
                //1.修改卡的表项，//取钱，
                record[1] = record[1] - money;//使用计算的余额减去要取余额。
                CardsAccess.UpdateCardsData((int)dAndC.Dcid, record[1]);//更新余额
                AddRecordsService(dAndC, CardsAccess.CardsData((int)dAndC.Dcid).Cicid, identity, money);//增加记录
                return true;
            }
            else return false;//不可取
        }

        /// <summary>
        /// 记录表Records中增加记录，业务逻辑层,CardsService对象将任务交给recordsService来处理
        /// </summary>
        /// <param name="dAndC">传入从cooike中获取的储户和储户银行卡信息。其对象为DepositorAndCard</param>
        /// <param name="v">出入参数v代表类型,1：代表取款，2：代表活期存款，其他：代表定期存款。每次传入一个类型的值，其他两项字段默认为0</param>
        /// <param name="money">金额</param>
        /// <param name="mid">业务办理员</param>
        public void AddRecordsService(DepositorAndCard dAndC, int Icid, int v, double money)
        {
            //此处零代表的是记录表中Mid填为0，代表取款是在ATM中进行的。
            recordsService.AddRecordsService(dAndC, Icid, v, money, 0);
        }






        #endregion

        #region 银行卡计算利息和余额
        /// <summary>
        /// 是CardsService的方法，银行卡计算利息和余额，没有更新数据库功能。只负责查询
        /// </summary>
        /// <param name="cid">那个卡？</param>
        /// <returns>返回计算后的余额，和利息</returns>
        public List<double> FlowBalanceService(int cid)
        {
            Cards card = new Cards();
            card = CardsAccess.CardsData(cid);
            if (card != null && card.Cid == cid)
            {
                //计算利息
                List<Double> record = new List<Double>();
                double balances = 0;//存款
                double rates = 0;//利息
                double balance = (double)card.CflowBalance;//取得卡表中活期现有存款
                double rate = (double)card.CflowBalanceRate / 360;//取得相应利率
                DateTime dt2 = System.DateTime.Now;//生成新的系统时间

                Double Day = 0;//记录相差天数
                DateTime dt1 = recordsService.RecordsTimeService(cid);//从records表中取得上次对活期存款操作的最后时间
                DateTime dt3 = transferRecordsService.RecordsTimeData(cid);//从转账记录中找到最后一次交易时间
                if (dt3 != DateTime.MinValue && dt1 != DateTime.MinValue)
                {
                    if (DateTime.Compare(dt3, dt1) > 0)
                    {
                        //Day = dt2.Day - dt2.Day;//
                        Day = DateDiff(dt3, dt2);
                    }
                    else
                    {
                        //Day = dt2.Day - dt1.Day;//天数差值
                        Day = DateDiff(dt1, dt2);
                    }
                }
                else if (dt3 != DateTime.MinValue) {
                    //Day = dt2.Day - dt3.Day;
                    Day = DateDiff(dt3, dt2);
                }
                else if (dt3 == DateTime.MinValue && dt1 == DateTime.MinValue)
                { Day = 0; }
                rates = (double)rate * Day * balance;//计算利息
                balances = rates + balance;
                //list表中加入我们要返回的数据
                record.Add(rates);
                record.Add(balances);
                return record;
            }
            else return null;
        }
        #endregion

        #region 计算天数
        private static int DateDiff(DateTime dateStart, DateTime dateEnd)
        {
            DateTime start = Convert.ToDateTime(dateStart.ToShortDateString());
            DateTime end = Convert.ToDateTime(dateEnd.ToShortDateString());

            TimeSpan sp = end.Subtract(start);

            return sp.Days;
        } 
        #endregion
        #region 注册银行卡
        /// <summary>
        /// 注册银行卡 业务逻辑层，是CardsService类的函数
        /// </summary>
        /// <param name="card">前端页面填写的信息：CflowBalanceRate，Cpassword，Cuid</param>
        internal void AddCardService(Cards card)
        {
            CardsAccess.AddData(card);
        }
        #endregion

        #region   活期存款
        /// <summary>
        /// 活期存款
        /// </summary>
        /// <param name="dAndC">cooike中当前账户信息：Cid，Uid，Name</param>
        /// <param name="money">前端出入ATM数钱器输入的活期存款金额</param>
        public void AddFloatBalanceService(DepositorAndCard dAndC, double money)
        {
            double moneys = money;//记录存款额
            //money += (double)CardsAccess.CardsData((int)dAndC.Dcid).CflowBalance;//将存款额和原来的余额加起来，等待更新
            money += FlowBalanceService((int)dAndC.Dcid)[1];
            CardsAccess.UpdateCardsData((int)dAndC.Dcid, money);//更新余额
            recordsService.AddRecordsService(dAndC, CardsAccess.CardsData((int)dAndC.Dcid).Cicid, 2, moneys, 0);//增加记录。0代表ATM
        }
        #endregion


        #region 查询银行卡信息
        /// <summary>
        /// 查询银行卡信息,根据银行卡账号查询
        /// </summary>
        /// <param name="cid">传入卡号cid</param>
        /// <returns></returns>
        public Cards CheakCardsService(int cid)
        {
            return CardsAccess.CardsData(cid);
        }
        #endregion

        #region 更新银行卡活期存款余额
        /// <summary>
        /// 更新银行卡活期存款余额
        /// </summary>
        /// <param name="card">传入修改后的银行卡信息</param>
        internal void UpdataFlowBalanceService(Cards card)
        {
            CardsAccess.UpdateCardsData(card.Cid, (double)card.CflowBalance);
        }
        #endregion

    }
}
