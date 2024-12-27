import 'package:apptmns/logic_constants/api_endpoints.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';
import 'package:get/get.dart';

import '../../../controllers/cart_controller.dart';
import '../../../core/components/network_image.dart';
import '../../../core/constants/constants.dart';

class SingleCartItemTile extends StatelessWidget {
  const SingleCartItemTile({
    super.key,
    required this.productId,
  });

  final String productId;

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => CartController());
    final cartController = Get.find<CartController>();

    int indexProduct = cartController.getIndexOfSingleItemFromCart(productId);

    return Padding(
      padding: const EdgeInsets.symmetric(
        horizontal: AppDefaults.padding,
        vertical: AppDefaults.padding / 2,
      ),
      child: Column(
        children: [
          Row(
            children: [
              /// Thumbnail
              SizedBox(
                width: 70,
                child: AspectRatio(
                  aspectRatio: 1 / 1,
                  child: NetworkImageWithLoader(
                    ApiEndpoints.apiUri+cartController.listCartItem[indexProduct]['productImage'],
                    fit: BoxFit.contain,
                  ),
                ),
              ),
              const SizedBox(width: 16),

              /// Quantity and Name
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Padding(
                    padding: const EdgeInsets.only(left: 8),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          cartController.listCartItem[indexProduct]['productName'],
                          style: Theme.of(context)
                              .textTheme
                              .bodyLarge
                              ?.copyWith(color: Colors.black),
                        ),
                        Text(
                          cartController.listCartItem[indexProduct]['price'].toString(),
                          style: Theme.of(context).textTheme.bodySmall,
                        ),
                      ],
                    ),
                  ),
                  Row(
                    children: [
                      IconButton(
                        onPressed: () {
                          cartController.setQuantityAndTotalPriceOfItem(productId, cartController.listCartItem[indexProduct]['quantity']+1);
                        },
                        icon: SvgPicture.asset(AppIcons.addQuantity),
                        constraints: const BoxConstraints(),
                      ),
                      Padding(
                        padding: const EdgeInsets.all(8.0),
                        child: Obx(() => Text(
                          cartController.listCartItem[indexProduct]['quantity'].toString(),
                          style: Theme.of(context)
                              .textTheme
                              .bodyLarge
                              ?.copyWith(
                            fontWeight: FontWeight.bold,
                            color: Colors.black,
                          ),
                        ),)
                      ),
                      IconButton(
                          onPressed: () {
                            cartController.setQuantityAndTotalPriceOfItem(productId, cartController.listCartItem[indexProduct]['quantity']-1);
                          },
                          icon: SvgPicture.asset(AppIcons.removeQuantity),
                          constraints: const BoxConstraints(),
                      ),

                    ],
                  )
                ],
              ),
              const Spacer(),

              /// Price and Delete labelLarge
              Column(
                children: [
                  IconButton(
                    constraints: const BoxConstraints(),
                    onPressed: () {
                      cartController.removeFromCart(productId);
                    },
                    icon: SvgPicture.asset(AppIcons.delete),
                  ),
                  const SizedBox(height: 16),
                  Text(
                    cartController.listCartItem[indexProduct]['totalPrice'].toString(),
                  ),
                ],
              )
            ],
          ),
          const Divider(thickness: 0.1),
        ],
      ),
    );
  }
}
