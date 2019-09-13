﻿using Business.bases;
using Common;
using Common.exception;
using DataAccess.DAO;
using log4net;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace Business.ClassBusiness
{
    public class CardListBussiness : AbstractBaseBusiness<CardList, string>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CardListBussiness()
            : base(new CardListsDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(CardList obj)
        {
            try
            {
                base.AddNew(obj);
                DataContext.SaveChanges();
            }
            catch (FaultException ex)
            {
                //write log
                _logger.Error("", ex);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.Error("", ex);
                if (ex is SqlException)
                {
                    throw ObjectUtil.CreateFaultException(CodedException.SqlExceptionUnhandler, Constants.Error.SqlException);
                }
                throw ObjectUtil.CreateFaultException(CodedException.Unhandler, Constants.Error.SqlUnhandler);
                //write log
            }
        }

        public override void Edit(CardList obj)
        {
            try
            {
                base.Edit(obj);
                DataContext.SaveChanges();
            }
            catch (SqlException ex)
            {
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.SqlExceptionUnhandler, "Sql Exeption");
            }
            catch (Exception ex)
            {
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.Unhandler, "Unhandler Exception");
            }
        }

        public override void Remove(string id)
        {
            try
            {
                base.Remove(id);
                DataContext.SaveChanges();
            }
            catch (SqlException ex)
            {
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.SqlExceptionUnhandler, "Sql Exeption");
            }
            catch (Exception ex)
            {
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.Unhandler, "Unhandler Exception");
            }
        }

        public int CheckCodeCreditCard(string code, bool checkIdMember)
        {
            var cardList = GetDynamicQuery().Where(x => x.ID == code).ToList().FirstOrDefault();

            if (cardList != null)
            {
                if (checkIdMember == true)
                {
                    if (cardList.Status == 0)
                    {
                        //thẻ chua active
                        cardList.Status = 1;
                        DataContext.SaveChanges();
                        return 1;
                    }
                    else
                    {
                        //thẻ đã active
                        return 0;
                    }
                }
                else if (checkIdMember == false && cardList.Status == 1)
                {
                    return 0;
                }
                else
                {
                    return 3;//Moi member chi co mot the
                }
            }
            return 2;//Ma the ko dung
        }

        public void CheckValidate(Comment obj)
        {
        }
    }
}