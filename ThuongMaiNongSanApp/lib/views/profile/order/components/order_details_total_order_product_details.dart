import 'package:apptmns/models/order_history.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../../controllers/order_history_controller.dart';
import '../../../../core/constants/constants.dart';
import 'order_details_product_tile.dart';

class TotalOrderProductDetails extends StatelessWidget {
  const TotalOrderProductDetails({
    super.key,
    required this.orderId,
  });
  final String orderId;
  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => OrderHistoryController());
    OrderHistoryController orderHistoryController = Get.find<OrderHistoryController>();
    List<OrderDetails> data = orderHistoryController.getOrderDetailFromOrderId(orderId);

    return Padding(
      padding: const EdgeInsets.all(AppDefaults.padding),
      child: Column(
        children: [
          const SizedBox(height: 8),
          Align(
            alignment: Alignment.centerLeft,
            child: Text(
              'Chi tiết sản phẩm',
              style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                    fontWeight: FontWeight.bold,
                    color: Colors.black,
                  ),
            ),
          ),
          const SizedBox(height: 8),
          Obx(() => ListView.separated(
            itemBuilder: (context, index) {
              return OrderDetailsProductTile(data: data[index]);
            },
            separatorBuilder: (context, index) => const Divider(
              thickness: 0.2,
            ),
            itemCount: orderHistoryController.getOrderDetailFromOrderId(orderId).length,
            shrinkWrap: true,
            physics: const NeverScrollableScrollPhysics(),
          ),)
        ],
      ),
    );
  }
}
