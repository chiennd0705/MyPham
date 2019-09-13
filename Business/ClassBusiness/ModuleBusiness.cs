using Business.bases;
using Common;
using Common.exception;
using DataAccess.DAO;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Business.ClassBusiness
{
    public class ModuleBusiness : AbstractBaseBusiness<Common.Module, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public ModuleBusiness()
            : base(new ModuleDao())
        {
            daoLayer.DataContext = DataContext;
        }

        /// <summary>
        /// Phương thức trả về danh sách module không thuộc role chỉ định
        /// </summary>
        /// <param name="roleId">RoleID</param>
        /// <returns>Danh sách module không thuộc role</returns>
        public List<Common.Module> GetModuleByNotInRole(long roleId)
        {
            try
            {
                var roleModuleBusiness = new RoleModuleBusiness();
                List<RoleModule> listModulesInRole = roleModuleBusiness.GetByRoleId(roleId);

                var arrayIdModulesInRole = new long[listModulesInRole.Count()];

                int i = 0;
                foreach (var re in listModulesInRole)
                {
                    arrayIdModulesInRole[i] = re.ModuleId;
                    i++;
                }

                IQueryable<Common.Module> query = GetDynamicQuery();
                return query.Where(p => arrayIdModulesInRole.Contains(p.Id) == false).OrderBy(p => p.Code).ThenBy(p => p.Order).ToList();
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
    }
}