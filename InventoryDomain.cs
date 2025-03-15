using System;
using System.Linq;
using LinqToDB;

public class InventoryDomain
{
    public object GetInventoryList(
        string warehouseCode,
        string soOrder,
        string mitemCode,
        string soOrderLine,
        string markingScheme)
    {
        using (var db = new MeiCloudDb())
        {
            var query = from ioq in db.INV_ONHAND_QTY
                        join sm in db.SFC_MITEM 
                        on ioq.MITEM_CODE equals sm.MITEM_CODE
                        where ioq.MITEM_CODE == mitemCode
                        && ioq.SO_ORDER == soOrder
                        && ioq.WAREHOUSE_CODE == warehouseCode
                        select new {
                            ioq.WAREHOUSE_CODE,
                            ioq.SO_ORDER,
                            ioq.SO_ORDER_LINE,
                            ioq.MARKING_SCHEME,
                            ioq.BATCH_NO,
                            ioq.MITEM_CODE,
                            sm.MITEM_NAME,
                            ioq.QTY
                        };

            // 应用可选过滤条件
            if (!string.IsNullOrEmpty(soOrderLine))
                query = query.Where(x => x.SO_ORDER_LINE == soOrderLine);
            if (!string.IsNullOrEmpty(markingScheme))
                query = query.Where(x => x.MARKING_SCHEME == markingScheme);

            // 分组并计算总量
            var result = query.GroupBy(x => new {
                x.WAREHOUSE_CODE,
                x.SO_ORDER,
                x.SO_ORDER_LINE,
                x.MARKING_SCHEME,
                x.BATCH_NO,
                x.MITEM_CODE,
                x.MITEM_NAME
            })
            .Select(g => new {
                g.Key.WAREHOUSE_CODE,
                g.Key.SO_ORDER,
                g.Key.SO_ORDER_LINE,
                g.Key.MARKING_SCHEME,
                g.Key.BATCH_NO,
                g.Key.MITEM_CODE,
                g.Key.MITEM_NAME,
                TotalQty = g.Sum(x => x.QTY)
            })
            .ToList();

            return new {
                success = true,
                data = result
            };
        }
    }
}
