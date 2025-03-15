using System;
using Newtonsoft.Json.Linq;

[ServiceDomain(UriTemplate = "/inventory")]
public class InventoryService : ServiceBase
{
    private readonly InventoryDomain _domain = new InventoryDomain();

    [RESTful(UriTemplate = "/GetInventoryList", Method = RequestMethod.GET)]
    public object GetInventoryList(
        string warehouseCode, 
        string soOrder, 
        string mitemCode,
        string soOrderLine = null, 
        string markingScheme = null)
    {
        // 参数验证
        if (string.IsNullOrEmpty(warehouseCode))
            throw new ArgumentException("仓库编码不能为空");
        if (string.IsNullOrEmpty(soOrder))
            throw new ArgumentException("销售订单号不能为空");
        if (string.IsNullOrEmpty(mitemCode))
            throw new ArgumentException("物料编码不能为空");

        // 调用Domain层处理
        return _domain.GetInventoryList(
            warehouseCode,
            soOrder,
            mitemCode,
            soOrderLine,
            markingScheme);
    }
}
