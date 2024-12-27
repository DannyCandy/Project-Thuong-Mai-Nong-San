import 'package:apptmns/controllers/order_history_controller.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../../core/constants/constants.dart';

class TotalAmountAndPaidData extends StatelessWidget {
  const TotalAmountAndPaidData({
    super.key,
    required this.orderId,
  });
  final String orderId;
  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => OrderHistoryController());
    OrderHistoryController orderHistoryController = Get.find<OrderHistoryController>();
    Map<String,String> data = orderHistoryController.getTotalAmountAndMethodPaidFromOrderId(orderId);

    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: AppDefaults.padding),
      child: Column(
        children: [
          Row(
            children: [
              Text(
                'Tổng giá trị',
                style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                    fontWeight: FontWeight.bold, color: Colors.black),
              ),
              const Spacer(),
              Text(
                '${data['totalAmount']} VND',
                style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                    fontWeight: FontWeight.bold, color: Colors.black),
              ),
            ],
          ),
          const SizedBox(height: 8),
          Row(
            children: [
              Text(
                'Phương thức',
                style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                    fontWeight: FontWeight.bold, color: Colors.black),
              ),
              const Spacer(),
              Text(
                data['methodPaid']!,
                style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                    fontWeight: FontWeight.bold, color: Colors.black),
              ),
            ],
          ),
        ],
      ),
    );
  }
}
