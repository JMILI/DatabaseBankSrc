using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankDepositUI.Models;
using BankDeposit.Model.SqlBank;
using BankDeposit.Service;
using System.Web.Script.Serialization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using BankDeposit.Model.Helper;
using System.IO;
using System.Web;
using NPOI.HSSF.UserModel;
using System.Security.Cryptography;
using System.Text;
using NPOI.SS.UserModel;

namespace BankDepositUI.Controllers
{
    public class DepositorsController : Controller
    {
        #region 实例化一些工具对象
        public static DepositorsService depositorServive = new DepositorsService();

        public static Transferrecords transferrecords = new Transferrecords();
        #endregion

        #region 辅助函数，导出数据用的
        /// <summary>
        /// 根据Excel列类型获取列的值
        /// </summary>
        /// <param name="cell">Excel列</param>
        /// <returns></returns>
        private static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }
        #endregion

        #region “登录”功能 已实现
        /// <summary>
        /// “登录”功能
        /// </summary>
        /// <param name="depositor">传入的是DepositorAndCard类型的数据</param>
        /// <returns>返回主页</returns>
        public IActionResult Login(DepositorAndCard depositor)
        {
            cooikeAdd(depositor);
            return View("Login", depositor);
        }
        #endregion

        #region  “注册储户”功能 已实现
        /// <summary>
        /// 返回页面提供用户填写信息
        /// </summary>
        /// <returns></returns>
        public IActionResult AddInformation()
        {
            return View();
        }
        //2.将返回的信息进行处理，然后登陆系统主页
        /// <summary>
        /// 增加储户，并登录到储户系统
        /// </summary>
        /// <param name="depositor">传入前端页面填写的信息，参数对象Depositors：Uid，Uicid，Upassword </param>
        /// <returns></returns>
        public IActionResult AddLogin(Depositors depositor)
        {
            depositor.Upassword = MD5Encrypt64(depositor.Upassword);
            DepositorAndCard depositors = new DepositorAndCard();
            depositors = depositorServive.AddService(depositor);
            if (depositors != null)
            {
                cooikeAdd(depositors);
                return RedirectToAction("Login", "Depositors", depositors);
            }
            else
                return Redirect(Url.Action("DepositoryExistError", "Errors"));
        }
        #endregion

        #region 转账记录查询
        /// <summary>
        /// 主要查询记录表中该用户的交易记录,查询前十项记录，该功能要用Ajax，
        /// 所以不需要建一个新的cshtml页面，只需要在Login的页面中加入数据即可。
        /// </summary>
        /// <returns>返回json类型数据，数据为十项记录</returns>
        public ActionResult QueryTenTransferRecords()
        {
            List<Transferrecords> record = new List<Transferrecords>();
            record = depositorServive.TenTransferRecordsService((int)DAndC().Dcid);//接受查询结果，为Records表的数据，十项记录
            return Content(JsonConvert.SerializeObject(record));//返回Json数据对象，Ajax接受
        }

        #region 记录导出Excel
        /// <summary>
        /// 记录导出Excel
        /// </summary>
        /// <returns></returns>
        public FileResult OutExcelTransfer()
        {
            //引入NPOI包，System.Configuration.ConfigurationManager包
            List<Transferrecords> record = new List<Transferrecords>();
            record = depositorServive.TenTransferRecordsService((int)DAndC().Dcid);//接受查询结果，为Records表的数据，十项记录
            //创建Excel文件的对象
            HSSFWorkbook book = new HSSFWorkbook();
            ////添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //貌似这里可以设置各种样式字体颜色背景等，
            //推荐：使用NPOI操作Excel导入导出数据
            //使用 NPOI 的优势: 1、你不需要在服务器上安装微软的 Office，可以避免版权问题。 2、使用起来比 Office PIA的 API更加方便，更人性化。 3、你不用去花大力气维
            //但是不是很方便，这里就不设置了
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("单号");
            row1.CreateCell(1).SetCellValue("转账方账号");
            row1.CreateCell(2).SetCellValue("转账人姓名");
            row1.CreateCell(3).SetCellValue("收账方账号");
            row1.CreateCell(4).SetCellValue("收账人姓名");
            row1.CreateCell(5).SetCellValue("转账额");
            row1.CreateCell(6).SetCellValue("交易时间");
            row1.CreateCell(7).SetCellValue("业务办理员账号");
            //....N行
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < record.Count; i++)
            {
                //    //创建行
                NPOI.SS.UserModel.IRow rowTemp = sheet1.CreateRow(i + 1);
                rowTemp.CreateCell(0).SetCellValue(record[i].Tid);
                rowTemp.CreateCell(1).SetCellValue(record[i].TpartyAcid);
                rowTemp.CreateCell(2).SetCellValue(record[i].TpartyAname);
                rowTemp.CreateCell(3).SetCellValue((double)record[i].TpartyBcid);
                rowTemp.CreateCell(4).SetCellValue(record[i].TpartyBname);
                rowTemp.CreateCell(5).SetCellValue((double)record[i].TtransferBalance);
                rowTemp.CreateCell(6).SetCellValue(record[i].TtransferTime.ToString());
                if ((int)record[i].Tmid == 0)
                {
                    rowTemp.CreateCell(7).SetCellValue("ATM");
                }
                else if ((int)record[i].Tmid == 1)
                {
                    rowTemp.CreateCell(7).SetCellValue("储户");
                }
                else
                    rowTemp.CreateCell(7).SetCellValue((int)record[i].Tmid);
                //    //....N行
            }
            // 写入到客户端 
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            DateTime dt = DateTime.Now;
            string dateTime = dt.ToString("yyMMddHHmmssfff");
            string fileName = "储户最近十项转账记录（最多10项）" + dateTime + ".xls";
            return File(ms, "application/vnd.ms-excel", fileName);
        }
        #endregion
        #endregion

        #region “查询近十笔交易记录”功能 已实现
        /// <summary>
        /// 主要查询记录表中该用户的交易记录,查询前十项记录，该功能要用Ajax，
        /// 所以不需要建一个新的cshtml页面，只需要在Login的页面中加入数据即可。
        /// </summary>
        /// <returns>返回json类型数据，数据为十项记录</returns>
        public ActionResult QueryTenRecords()
        {
            List<Records> record = new List<Records>();
            record = depositorServive.TenRecordsService((int)DAndC().Dcid);//接受查询结果，为Records表的数据，十项记录
            return Content(JsonConvert.SerializeObject(record));//返回Json数据对象，Ajax接受
        }

        #region 记录导出Excel
        /// <summary>
        /// 记录导出Excel
        /// </summary>
        /// <returns></returns>
        public FileResult OutExcel()
        {
            //引入NPOI包，System.Configuration.ConfigurationManager包
            List<Records> record = new List<Records>();
            record = depositorServive.TenRecordsService((int)DAndC().Dcid);
            //创建Excel文件的对象
            HSSFWorkbook book = new HSSFWorkbook();
            ////添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //貌似这里可以设置各种样式字体颜色背景等，
            //推荐：使用NPOI操作Excel导入导出数据
            //使用 NPOI 的优势: 1、你不需要在服务器上安装微软的 Office，可以避免版权问题。 2、使用起来比 Office PIA的 API更加方便，更人性化。 3、你不用去花大力气维
            //但是不是很方便，这里就不设置了
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("单号");
            row1.CreateCell(1).SetCellValue("账户ID");
            row1.CreateCell(2).SetCellValue("银行卡号");
            row1.CreateCell(3).SetCellValue("活期存款额（元）");
            row1.CreateCell(4).SetCellValue("定期存款额（元）");
            row1.CreateCell(5).SetCellValue("取款额（元）");
            row1.CreateCell(6).SetCellValue("交易时间");
            row1.CreateCell(7).SetCellValue("办理员账号");
            //....N行
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < record.Count; i++)
            {
                //    //创建行
                NPOI.SS.UserModel.IRow rowTemp = sheet1.CreateRow(i + 1);
                rowTemp.CreateCell(0).SetCellValue(record[i].Rid);
                rowTemp.CreateCell(1).SetCellValue(record[i].Ruid);
                rowTemp.CreateCell(2).SetCellValue(record[i].Rcid);
                rowTemp.CreateCell(3).SetCellValue((double)record[i].RflowDeposit);
                rowTemp.CreateCell(4).SetCellValue((double)record[i].RfixDeposit);
                rowTemp.CreateCell(5).SetCellValue((double)record[i].Rwithdrawals);
                rowTemp.CreateCell(6).SetCellValue(record[i].RnowDateTime.ToString());
                rowTemp.CreateCell(7).SetCellValue((double)record[i].Rmid);
                //    //....N行
            }
            // 写入到客户端 
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            DateTime dt = DateTime.Now;
            string dateTime = dt.ToString("yyMMddHHmmssfff");
            string fileName = "储户最近十项交易记录（最多10项）" + dateTime + ".xls";
            return File(ms, "application/vnd.ms-excel", fileName);
        }
        #endregion

        #endregion

        #region “绑定银行卡”功能 已实现
        /// <summary>
        /// 返回绑定银行卡的填写页面，此功能是绑定银行卡，而不是选绑银行卡
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdataBandInformation()
        {
            return View();
        }
        //2.将返回的信息进行处理，然后登陆系统主页
        /// <summary>
        /// 绑定银行卡并返回主页
        /// </summary>
        /// <param name="depositor">前端传入DepositorAndCard对象，属性：Dcid,Dicid</param>
        /// <returns></returns>
        public IActionResult UpdataBand(DepositorAndCard depositor)
        {
            DepositorAndCard depositor1 = new DepositorAndCard();
            depositor.Duid = DAndC().Duid;
            depositor1 = depositorServive.UpdataBandService(depositor);//绑定
            if (depositor1 != null)
            {
                return RedirectToAction("Login", "Depositors", depositor1);
            }
            else
            {
                return Redirect(Url.Action("DepositoryNotExistError", "Errors"));
            }
        }

        #endregion

        #region “查询活期余额”功能 已实现
        public ActionResult FlowBalance()
        {
            List<Double> record = new List<Double>();//double类型的List，存放一个余额，一个利息
            record = depositorServive.FlowBalanceService((int)DAndC().Dcid);//这里需要service返回一个余额，一个利息
            return Content(JsonConvert.SerializeObject(record));//以json方式
        }
        #endregion

        #region “查询定期余额”功能 已实现
        public ActionResult FixBalance()
        {
            List<Fixbalances> record = new List<Fixbalances>();
            record = depositorServive.FixBalanceService((int)DAndC().Dcid);//这里需要service返回一个余额，一个利息
            return Content(JsonConvert.SerializeObject(record));//以json方式
        }
        #endregion

        #region “转账”功能 已完成
        public IActionResult TransferInformation()
        {
            if (DAndC().Dcid == 0)
            {
                return Content("<script>alert('您还没有绑定银行，请绑定银行卡');history.go(-1);</script>", "text/html;charset=UTF-8");
            }
            else
                return View();
        }

        /// <summary>
        /// 验证密码页面，填写密码
        /// </summary>
        /// <param name="transfer">包含转账的乙方账号，转账金额</param>
        /// <returns></returns>
        public IActionResult VerifyPassword(Transferrecords transfer)
        {
            transferrecords.TpartyBcid = transfer.TpartyBcid;//将数据交给静态数据对象，暂时存起来。
            transferrecords.TtransferBalance = transfer.TtransferBalance;
            ViewBag.Cid = transfer.TpartyBcid;//向前端展示交易信息，确认
            ViewBag.TtransferBalance = transfer.TtransferBalance;
            return View(ViewBag);
        }
        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="card">Cpassword</param>
        /// <returns></returns>
        public IActionResult Transfer(Cards card)
        {

            transferrecords.TpartyAcid = (int)DAndC().Dcid;//将转账甲方账号，姓名，给转账记录对象
            transferrecords.TpartyAname = DAndC().Dname;
            transferrecords.Tmid = 1;
            if (depositorServive.Transfer(transferrecords, MD5Encrypt64(card.Cpassword)) == true)//1:代表储户操作
            {
                return RedirectToAction("Success", "Depositors");
            }
            else
            {
                return  Content("<script>alert('密码错误');history.go(-1);</script>", "text/html;charset=UTF-8");

            }
        }
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult ReLogin()
        {
            return RedirectToAction("Login", "Depositors", DAndC());
        }
        #endregion

        #region 辅助函数：登录，注册功能模块中，加入cooike
        /// <summary>
        /// 向浏览器cooike中加入值DepositorAndCard
        /// </summary>
        /// <param name="card1">传入DepositorAndCard对象</param>
        public void cooikeAdd(DepositorAndCard depositor)
        {
            this.Response.Cookies.Append("Uid", depositor.Duid.ToString());
            this.Response.Cookies.Append("Cid", depositor.Dcid.ToString());
            this.Response.Cookies.Append("Name", depositor.Dname.ToString());
        }
        /// <summary>
        /// 从浏览器获取DepositorAndCard对象的值：Dcid，Dname，Duid
        /// </summary>
        /// <returns>返回DepositorAndCard对象</returns>
        public DepositorAndCard DAndC()
        {
            DepositorAndCard dAndC = new DepositorAndCard();
            this.Request.Cookies.TryGetValue("Cid", out string Cid);
            int cid = 0;
            if (Cid!="")
            {
                cid = int.Parse(Cid);
            }
            this.Request.Cookies.TryGetValue("Uid", out string Uid);
            int uid = int.Parse(Uid);
            this.Request.Cookies.TryGetValue("Name", out string Name);
            string name = Name;
            dAndC.Dcid = cid;
            dAndC.Dname = name;
            dAndC.Duid = uid;
            return dAndC;
        }
        #endregion

        #region 辅助函数MD5加密用户密码
        /// <summary>
        /// MD5加密用户密码
        /// </summary>
        /// <param name="password"> 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　</param>
        /// <returns></returns>
        public static string MD5Encrypt64(string password)
        {
            string cl = password;
            MD5 md5 = MD5.Create(); //实例化一个md5对像
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }
        #endregion

    }
}
