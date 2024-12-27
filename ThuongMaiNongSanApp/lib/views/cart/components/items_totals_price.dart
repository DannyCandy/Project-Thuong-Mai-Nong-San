import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../controllers/cart_controller.dart';
import '../../../core/components/dotted_divider.dart';
import '../../../core/constants/constants.dart';
import 'item_row.dart';

class ItemTotalsAndPrice extends StatelessWidget {
  const ItemTotalsAndPrice({
    super.key,
  });

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => CartController());
    final cartController = Get.find<CartController>();

    return Padding(
      padding: const EdgeInsets.all(AppDefaults.padding),
      child: Obx(() => Column(
        children: [
          ItemRow(
            title: 'Số sản phẩm',
            value: '${cartController.getNumOfItemInCart().toString()} sản phẩm',
          ),
          ItemRow(
            title: 'Giá',
            value: '${cartController.getTotalPriceOfCart().toString()} VND',
          ),
          const ItemRow(
            title: 'Giảm giá',
            value: '0 VND',
          ),
          const DottedDivider(),
          ItemRow(
            title: 'Tổng giá trị',
            value: '${cartController.getTotalPriceOfCart().toString()} VND',
          ),
        ],
      )),
    );
  }
}
