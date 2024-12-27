import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:get/get.dart';

import '../../controllers/cart_controller.dart';
import '../constants/constants.dart';

class PriceAndQuantityRow extends StatefulWidget {
  const PriceAndQuantityRow({
    super.key,
    required this.currentPrice,
    /*required this.orginalPrice,*/
    required this.quantity,
  });

  final int currentPrice;
  /*final double orginalPrice;*/
  final int quantity;

  @override
  State<PriceAndQuantityRow> createState() => _PriceAndQuantityRowState();
}

class _PriceAndQuantityRowState extends State<PriceAndQuantityRow> {
  int quantity = 1;
  late final CartController cartController;
  onQuantityIncrease() {
    quantity++;
    cartController.setQuantityOfCurrentItem(quantity);
    setState(() {});
  }

  onQuantityDecrease() {
    if (quantity > 1) {
      quantity--;
      cartController.setQuantityOfCurrentItem(quantity);
      setState(() {});
    }
  }

  @override
  void initState() {
    super.initState();
    Get.lazyPut(() => CartController());
    cartController = Get.find<CartController>();
    quantity = widget.quantity;
    WidgetsBinding.instance.addPostFrameCallback((timeStamp) {
      setState(() {});
    });
  }

  @override
  Widget build(BuildContext context) {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.end,
      children: [
        /* <---- Price -----> */
        /*Text(
          '\$30',
          style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                color: Colors.black,
                fontWeight: FontWeight.bold,
                decoration: TextDecoration.lineThrough,
              ),
        ),*/
        const SizedBox(width: AppDefaults.padding),
        Text(
          '${widget.currentPrice} VND',
          style: Theme.of(context)
              .textTheme
              .headlineSmall
              ?.copyWith(color: AppColors.primary, fontWeight: FontWeight.bold),
        ),
        const Spacer(),

        /* <---- Quantity -----> */
        Row(
          children: [
            IconButton(
              onPressed: onQuantityIncrease,
              icon: SvgPicture.asset(AppIcons.addQuantity),
              constraints: const BoxConstraints(),
            ),
            Padding(
              padding: const EdgeInsets.all(8.0),
              child: Text(
                '$quantity',
                style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                      fontWeight: FontWeight.bold,
                      color: Colors.black,
                    ),
              ),
            ),
            IconButton(
              onPressed: onQuantityDecrease,
              icon: SvgPicture.asset(AppIcons.removeQuantity),
              constraints: const BoxConstraints(),
            ),
          ],
        )
      ],
    );
  }
}
