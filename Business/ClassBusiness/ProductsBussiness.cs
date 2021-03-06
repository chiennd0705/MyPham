using Business.bases;
using Common;
using Common.exception;
using DataAccess.DAO;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace Business.ClassBusiness
{
    public class ProductsBusiness : AbstractBaseBusiness<Product, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ProductsBusiness()
            : base(new ProductsDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(Product obj)
        {
            try
            {
                CheckValidate(obj);
                obj.CreateDate = DateTime.Now;

                base.AddNew(obj);
                DataContext.SaveChanges();
            }
            catch (FaultException ex)
            {
                //write log
                _logger.Error("", ex);
                throw ex;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ObjectUtil.CreateFaultException(CodedException.NullEntity, "Vui lòng nhập các ô có dấu *");
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

        public List<Product> GetList(ref List<Product> listproduct, long memberId, string name, List<long> cate, int? state, int? statu, int? day)
        {
            var obj = GetDynamicQuery().Where(x => x.MemberId == memberId).OrderByDescending(x => x.ModifyDate).ToList();
            listproduct = obj;
            if (string.IsNullOrEmpty(name) && !cate.Any() && state == null && statu == null & day == null)
            {
                return obj;
            }
            if (!string.IsNullOrEmpty(name))
            {
                obj =
                obj.Where(
                        x =>
                            x.ProductName.Contains(name)).ToList();
                //   return obj;
            }
            if (cate.Any())
            {
                obj = obj.Where(
                         x => cate.Contains(x.CatalogId)).ToList();
                //return obj;
            }
            if (statu != -1 & statu != null)
            {
                obj = obj.Where(
                         x =>
                             x.Status == statu).ToList();
                //     return obj;
            }
            if (state != -1 && state != null)
            {
                obj = obj.Where(
                         x =>
                             x.State == state).ToList();
                //  return obj;
            }
            if (day != -1 && day != null)
            {
                var datetime = DateTime.Now.AddDays(-(int)day);
                obj = obj.Where(
                            x => x.CreateDate > datetime).ToList();
                //    return obj;
            }

            if (!string.IsNullOrEmpty(name) && cate.Any() && state != -1 && statu != -1 & day != -1)
            {
                var datetime = DateTime.Now.AddDays(-(int)day);
                obj = obj.Where(
                      x =>
                          x.ProductName.Contains(name) && cate.Contains(x.CatalogId) && x.State == state &&
                          x.Status == statu && x.CreateDate > datetime).ToList();
                // return obj;
            }

            return obj;
        }

        public override void Edit(Product obj)
        {
            try
            {
                CheckValidate(obj);

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

        public override void Remove(long id)
        {
            try
            {
                base.Remove(id);
                DataContext.SaveChanges();
            }
            catch (SqlException ex)
            {
                throw ObjectUtil.CreateFaultException(CodedException.SqlExceptionUnhandler, "Sql Exeption");
            }
            catch (Exception ex)
            {
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.Unhandler, "Unhandler Exception");
            }
        }

        public void UpdateStatus(string[] array, int st)
        {
            foreach (var item in array)
            {
                var obj = GetById(long.Parse(item));
                obj.Status = st;
                Edit(obj);
            }
        }

        public void RemoveAll(string[] array)
        {
            foreach (var item in array)
            {
                Remove(long.Parse(item));
            }
        }

        public bool CheckArayId(string[] array, long memberId)
        {
            var bl = true;
            foreach (var item in array)
            {
                var id = long.Parse(item);
                var obj = GetDynamicQuery().Where(x => x.Id == id && x.MemberId == memberId).ToList();
                if (!obj.Any())
                {
                    bl = false;
                }
            }
            return bl;
        }

        public List<Product> SearchProductByBrandId(long brandId, int size)
        {
            IQueryable<Product> query = GetDynamicQuery();
            var list = query.Where(p => p.ManufacturerId == brandId && p.Status == 1).OrderByDescending(p => p.ModifyDate).Take(size).ToList();
            return list;
        }

        public List<SearchProductByName1_Result> SearchProductName(string key)
        {
            var ProductEntities = new BuyGroup365Entities();

            var listResults = ProductEntities.SearchProductByName1(key).ToList();

            //var ProductEntities = new BuyGroup365Entities();

            //  var listResults = ProductEntities().Where(p => p. == brandId && p.Status == 1).OrderByDescending(p => p.ModifyDate).Take(size).ToList(); ;
            //   var listResults = GetDynamicQuery().Where(x => x.ProductName.Contains(key)).Select(x => new { ProductName = x.ProductName, Price = x.Price, ImgSource = x.ProductImages.Where(z => z.IsAvatar == 1).Select(z => z.ImgSource) }).ToArray();

            return listResults;
        }

        public List<Product> GetByKey(string key, int? statusProduct, int? isOfProduct, long? shopid, long? CatalogIDProduct)
        {
            var ProductEntities = new BuyGroup365Entities();
            List<Product> ListPro = new List<Product>();
            IQueryable<Product> listResults = GetDynamicQuery();
            if (CatalogIDProduct != -1)
            {
                Product pro1 = new Product();
                var listcatalog = ProductEntities.CatalogProducts.Where(p => p.CatalogId == CatalogIDProduct).ToList();
                foreach (var re in listcatalog)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        var obj = GetById(re.ProductId);
                        if (obj.ProductName.Contains(key) || obj.Code.Contains(key))
                            ListPro.Add(obj);
                    }
                    else
                    {
                        var obj = GetById(re.ProductId);
                        ListPro.Add(obj);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(key))
                {
                    ListPro = listResults.Where(x => x.ProductName.Contains(key)).ToList();
                }
                else
                {
                    ListPro = listResults.ToList();
                }
            }

            return ListPro;
        }

        protected string BuildQuery(long? id, string mid, string price, string order, int? page, string param, string k, int Page_Size)
        {
            string query = "", buildQuery = "";
            if (mid == "" || mid == null) mid = null;
            else
            {
                mid = "AND ManufacturerId in (" + mid + ")";
            }
            if (price != "" && price != null)
            {
                string[] pricelist = price.Split(',');
                string PriceTemp = "AND (";
                int i = 0;
                foreach (string a in pricelist)
                {
                    if (i == 0)
                        PriceTemp += "((" + a.Split(':')[0] + " <=Price)  AND (" + a.Split(':')[1] + " >=Price)) ";
                    else
                        PriceTemp += "OR ((" + a.Split(':')[0] + " <=Price)  AND (" + a.Split(':')[1] + " >=Price)) ";
                    i++;
                }
                PriceTemp += ")";
                price = PriceTemp;
            }
            if (id != 0)
            {
                query = @"BEGIN
				            WITH temp(id)
			                as (

					                Select Id
					                From Catalogs
					                Where Id ={0}
					                Union All
					                Select b.Id
					                From temp as a, Catalogs as b
					                Where a.Id = b.ParentId

			                )
			                 SELECT distinct *
				                FROM(
				                select *
				                , ROW_NUMBER() OVER(ORDER BY  Id ) RN
								FROM (
								SELECT  distinct Products.Id, ProductName,Summary, FriendlyUrl, [dbo].[FindAvatar](Products.Id) AS ImgSource, ColWidth, Code,Price, Cost, IsNew,IsProductBig,Tags, Model,ManufacturerId, IsAttractive, IsVip,LongImages,IsHome, TotalView, [dbo].[CaculatorRate](Products.Id) AS Rate,ModifyDate,LandingPage

				                       from temp join CatalogProducts ON temp.Id=CatalogProducts.CatalogId join  Products ON CatalogProducts.ProductId = Products.Id
				                           LEFT JOIN ProductProperties ON Products.Id=ProductProperties.ProductId
				                WHERE (Status = 2 OR Status=3 OR Status=4)
				             {1}
                            {2}
                                AND (ProductName Like N'%{8}%' OR Tags Like N'%{8}%'  OR Code Like N'%{8}%')
				              {3} )b
			                ) a
				                WHERE RN BETWEEN ({4}-1)*{7}+1 AND {4}*{7}
				                ORDER BY {6}
		                END";
                if (order.Equals("new"))//Mới nhất
                    buildQuery = string.Format(query, id, mid, price, param, page, "Products.Id desc", "Id desc", Page_Size, k);
                else if (order.Equals("name"))
                    buildQuery = string.Format(query, id, mid, price, param, page, "ProductName desc", "ProductName desc", Page_Size, k);
                else if (order.Equals("view"))
                    buildQuery = string.Format(query, id, mid, price, param, page, "TotalView desc", "TotalView desc", Page_Size, k);
                else if (order.Equals("price_asc"))
                    buildQuery = string.Format(query, id, mid, price, param, page, "Price asc", "Price asc", Page_Size, k);
                else if (order.Equals("price_desc"))
                    buildQuery = string.Format(query, id, mid, price, param, page, "Price desc", "Price desc", Page_Size, k);
            }
            else
            {
                query = @"BEGIN

			                 SELECT *
				                FROM(
				                select *
				                , ROW_NUMBER() OVER(ORDER BY  {6} ) RN
								FROM (
								SELECT  distinct Products.Id, ProductName,Summary, FriendlyUrl, [dbo].[FindAvatar](Products.Id) AS ImgSource, ColWidth, Code,Price, Cost, IsNew,ManufacturerId,IsProductBig,Tags, Model, IsAttractive, IsVip,LongImages,IsHome, TotalView, [dbo].[CaculatorRate](Products.Id) AS Rate,ModifyDate,LandingPage

				                from  Products  LEFT JOIN ProductProperties ON Products.Id=ProductProperties.ProductId
				                WHERE (Status = 2 OR Status=3 OR Status=4)
				              {1}
				              {2}
                                AND (ProductName Like N'%{8}%' OR Tags Like N'%{8}%' OR Code Like N'%{8}%')
				               {3})b
			                  ) a
				                WHERE RN BETWEEN ({4}-1)*{7}+1 AND {4}*{7}
				                ORDER BY {6} {0}
		                END";
                if (order.Equals("new"))//Mới nhất
                    buildQuery = string.Format(query, "", mid, price, param, page, "Products.Id desc", "Id desc", Page_Size, k);
                else if (order.Equals("name"))
                    buildQuery = string.Format(query, "", mid, price, param, page, "ProductName desc", "ProductName desc", Page_Size, k);
                else if (order.Equals("view"))
                    buildQuery = string.Format(query, "", mid, price, param, page, "TotalView desc", "TotalView desc", Page_Size, k);
                else if (order.Equals("price_asc"))
                    buildQuery = string.Format(query, "", mid, price, param, page, "Price asc", "Price asc", Page_Size, k);
                else if (order.Equals("price_desc"))
                    buildQuery = string.Format(query, "", mid, price, param, page, "Price desc", "Price desc", Page_Size, k);
            }

            return buildQuery;
        }

        protected string BuildQuery(long id, int page, int Page_Size)
        {
            string query = "";
            string buildQuery = "";
            if (id != 0)
            {
                query = @"BEGIN
				            WITH temp(id)
			                as (

					                Select Id
					                From Catalogs
					                Where Id ={0}
					                Union All
					                Select b.Id
					                From temp as a, Catalogs as b
					                Where a.Id = b.ParentId

			                )
			                 SELECT distinct *
				                FROM(
				                select *
				                , ROW_NUMBER() OVER(ORDER BY  Id ) RN
								FROM (
								SELECT  distinct Products.Id, ProductName,Summary, FriendlyUrl, [dbo].[FindAvatar](Products.Id) AS ImgSource, ColWidth, Code,Price, Cost, IsNew,IsProductBig,Tags, Model,ManufacturerId, IsAttractive, IsVip,LongImages,IsHome, TotalView, [dbo].[CaculatorRate](Products.Id) AS Rate,ModifyDate,LandingPage

				                       from temp join CatalogProducts ON temp.Id=CatalogProducts.CatalogId join  Products ON CatalogProducts.ProductId = Products.Id
				                           LEFT JOIN ProductProperties ON Products.Id=ProductProperties.ProductId
				                WHERE (Status = 2 OR Status=3 OR Status=4)
				           )b
			                ) a
				                WHERE RN BETWEEN ({1}-1)*{2}+1 AND {1}*{2}
				                ORDER BY ModifyDate Desc
		                END";

                buildQuery = string.Format(query, id, page, Page_Size);
            }

            return buildQuery;
        }

        public void CheckValidate(Product obj)
        {
        }

        public List<Product> GetByAttactive(int number_item)
        {
            var obj = GetDynamicQuery().Where(x => x.IsAttractive == true).OrderByDescending(x => x.ModifyDate).Take(number_item).ToList();
            return obj;
        }

        public List<Product> GetByNew(int number_item)
        {
            var obj = GetDynamicQuery().Where(x => x.IsNew == true).OrderByDescending(x => x.ModifyDate).Take(number_item).ToList();
            return obj;
        }

        public List<Product> GetBySeller(int number_item)
        {
            var obj = GetDynamicQuery().Where(x => x.IsVip == true).OrderByDescending(x => x.ModifyDate).Take(number_item).ToList();
            return obj;
        }

        public List<Product> ListByTypeProduct(int typeProduct, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            IQueryable<Common.Product> query = GetDynamicQuery();
            List<Product> getListById = null;
            if (typeProduct == 1)  // IsActive
                getListById = query.Where(x => x.IsAttractive == true).ToList();
            if (typeProduct == 2)  //IsNew
                getListById = query.Where(x => x.IsNew == true).ToList();
            if (typeProduct == 3)  //IsVip
                getListById = query.Where(x => x.IsVip == true).ToList();
            if (typeProduct == 4)  //IsProductBig
                getListById = query.Where(x => x.IsProductBig == true).ToList();
            totalRecord = getListById.Count();
            var model = getListById.ToList();
            model = model.OrderByDescending(x => x.ModifyDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return model;
        }

        public List<Product> GetByTag(string tag, int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<Common.Product> query = GetDynamicQuery();
            var getListById = query.Where(x => x.Tags.Contains(tag));

            var model = getListById.ToList();
            model = model.OrderByDescending(x => x.ModifyDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return model.ToList();
        }

        public int CountByTag(string tag)
        {
            int obj = GetDynamicQuery().Where(x => x.Tags.Contains(tag)).Count();
            return obj;
        }

        protected string BuildQuery(long? id, long? mid, double? priceFrom, double? priceTo, string order, int? page, int? Page_Size, string param, string k)
        {
            string query = "", buildQuery = "";

            if (id != 0)
            {
                query = @"BEGIN
				            WITH temp(id)
			                as (

					                Select Id
					                From Catalogs
					                Where Id ={0}
					                Union All
					                Select b.Id
					                From temp as a, Catalogs as b
					                Where a.Id = b.ParentId

			                )
			                 SELECT distinct *
				                FROM(
				                select *
				                , ROW_NUMBER() OVER(ORDER BY  Id ) RN
								FROM (
								SELECT  distinct Products.Id, ProductName,Summary, FriendlyUrl, [dbo].[FindAvatar](Products.Id) AS ImgSource, ColWidth, Code,Price,LongImages, Cost, IsNew,IsProductBig,ManufacturerId,Tags, Model, IsAttractive, IsVip,IsHome, TotalView, [dbo].[CaculatorRate](Products.Id) AS Rate,ModifyDate,LandingPage

				                       from temp join CatalogProducts ON temp.Id=CatalogProducts.CatalogId join  Products ON CatalogProducts.ProductId = Products.Id
				                           LEFT JOIN ProductProperties ON Products.Id=ProductProperties.ProductId
				                WHERE (Status = 2 OR Status=3 OR Status=4)

      AND ({2}=0 OR {2} <=Price)
				                AND ({3}=0 OR {3} >=Price)
                                AND (ProductName Like N'%{9}%' OR Tags Like N'%{9}%'  OR Code Like N'%{9}%')
				               )b
			                ) a
				                WHERE RN BETWEEN ({5}-1)*{8}+1 AND {5}*{8}
				                ORDER BY {7}
		                END";
                if (order.Equals("new"))//Mới nhất
                    buildQuery = string.Format(query, id, mid, priceFrom, priceTo, param, page, "IsProductBig desc,ModifyDate desc", "IsProductBig desc,ModifyDate desc", Page_Size, k);
                else if (order.Equals("name"))
                    buildQuery = string.Format(query, id, mid, priceFrom, priceTo, param, page, "ProductName desc", "ProductName desc", Page_Size, k);
                else if (order.Equals("view"))
                    buildQuery = string.Format(query, id, mid, priceFrom, priceTo, param, page, "TotalView desc", "TotalView desc", Page_Size, k);
                else if (order.Equals("price_asc"))
                    buildQuery = string.Format(query, id, mid, priceFrom, priceTo, param, page, "Price asc", "Price asc", Page_Size, k);
                else if (order.Equals("price_desc"))
                    buildQuery = string.Format(query, id, mid, priceFrom, priceTo, param, page, "Price desc", "Price desc", Page_Size, k);
            }
            else
            {
                query = @"BEGIN

			                 SELECT *
				                FROM(
				                select *
				                , ROW_NUMBER() OVER(ORDER BY  {7} ) RN
								FROM (
								SELECT  distinct Products.Id, ProductName,Summary, FriendlyUrl, [dbo].[FindAvatar](Products.Id) AS ImgSource, ColWidth, Code,Price,LongImages, Cost, IsNew,IsProductBig,ManufacturerId,Tags, Model, IsAttractive, IsVip,IsHome, TotalView, [dbo].[CaculatorRate](Products.Id) AS Rate,ModifyDate,LandingPage

				                from  Products  LEFT JOIN ProductProperties ON Products.Id=ProductProperties.ProductId
				                WHERE (Status = 2 OR Status=3 OR Status=4)
				                AND ({1}=0 OR ManufacturerId = {1})
				                AND ({2}=0 OR {2} <=Price)
				                AND ({3}=0 OR {3} >=Price)
                                AND (ProductName Like N'%{9}%' OR Tags Like N'%{9}%' OR Code Like N'%{9}%')
				               {4})b
			                  ) a
				                WHERE RN BETWEEN ({5}-1)*{8}+1 AND {5}*{8}
				                ORDER BY {7} {0}
		                END";
                if (order.Equals("new"))//Mới nhất
                    buildQuery = string.Format(query, "", mid, priceFrom, priceTo, param, page, "IsProductBig desc,ModifyDate desc", "IsProductBig desc,ModifyDate desc", Page_Size, k);
                else if (order.Equals("name"))
                    buildQuery = string.Format(query, "", mid, priceFrom, priceTo, param, page, "ProductName desc", "ProductName desc", Page_Size, k);
                else if (order.Equals("view"))
                    buildQuery = string.Format(query, "", mid, priceFrom, priceTo, param, page, "TotalView desc", "TotalView desc", Page_Size, k);
                else if (order.Equals("price_asc"))
                    buildQuery = string.Format(query, "", mid, priceFrom, priceTo, param, page, "Price asc", "Price asc", Page_Size, k);
                else if (order.Equals("price_desc"))
                    buildQuery = string.Format(query, "", mid, priceFrom, priceTo, param, page, "Price desc", "Price desc", Page_Size, k);
            }

            return buildQuery;
        }

        public List<SearchProductByType_Result> ListByProductsIdCatalogID(int id)
        {
            string urlParam = string.Empty;
            string k = "";
            long ids = 0;
            if (id == null) id = 0;
            int page = 1;
            int Page_Size = 30;
            double? priceForm = 0, priceTo = 0;
            string order = "new";
            string strQuery = BuildQuery(id, ids, priceForm, priceTo, order, page, Page_Size, urlParam, k);

            BuyGroup365Entities entitis = new BuyGroup365Entities();
            var result = entitis.Database.SqlQuery<SearchProductByType_Result>(strQuery).ToList();
            return result;
        }

        public List<SearchProductByName_Result> SearchListProductByName(string key)
        {
            var ProductEntities = new BuyGroup365Entities();

            var listResults = ProductEntities.SearchProductByName(key).ToList();

            return listResults;
        }

        public List<SearchProductByType_Result> GetBySellerByCatalogID(long id, int page = 0, int Page_Size = 3)
        {
            string strQuery = BuildQuery(id, page, Page_Size);

            BuyGroup365Entities entitis = new BuyGroup365Entities();
            var result = entitis.Database.SqlQuery<SearchProductByType_Result>(strQuery).ToList();
            return result;
        }

        public List<SearchProductByType_Result> ListByProductsIdCatalogIDActiveHome(int id, int size)
        {
            var listproduct = ListByProductsIdCatalogID(id).Where(x => x.IsHome = true).Take(size).ToList();

            return listproduct;
        }
    }
}