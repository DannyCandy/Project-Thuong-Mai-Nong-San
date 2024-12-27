import 'package:apptmns/controllers/product_controller.dart';
import 'package:apptmns/models/order_history.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../../core/components/network_image.dart';

class OrderDetailsProductTile extends StatelessWidget {
  const OrderDetailsProductTile({
    super.key,
    required this.data,
  });

  final OrderDetails data;

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => ProductController());
    ProductController productController = Get.find<ProductController>();

    return Row(
      children: [
        SizedBox(
          height: 80,
          child: AspectRatio(
            aspectRatio: 1 / 1,
            child: NetworkImageWithLoader(
              productController.getProductImageById(data.idsp!),
              fit: BoxFit.contain,
            ),
          ),
        ),
        const SizedBox(width: 16),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                productController.getProductNameById(data.idsp!),
                style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                      // fontWeight: FontWeight.bold,
                      color: Colors.black,
                    ),
              ),
              const SizedBox(height: 8),
              /*Text(data.weight)*/
            ],
          ),
        ),
        Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              '${data.price} VND',
              style: Theme.of(context).textTheme.titleLarge,
            ),
            const SizedBox(height: 8),
            Text(
              '${data.quantity}x',
              style: Theme.of(context).textTheme.bodySmall,
            )
          ],
        )
      ],
    );
  }
}
