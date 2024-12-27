import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../controllers/cart_controller.dart';
import '../../core/components/app_back_button.dart';
import '../../core/constants/app_defaults.dart';
import '../../core/routes/app_routes.dart';
import 'components/coupon_code_field.dart';
import 'components/items_totals_price.dart';
import 'components/single_cart_item_tile.dart';
import 'empty_cart_page.dart';

class CartPage extends StatelessWidget {
  const CartPage({
    super.key,
    this.isHomePage = false,
  });

  final bool isHomePage;

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => CartController());
    final cartController = Get.find<CartController>();


    return Scaffold(
      appBar: isHomePage
          ? null
          : AppBar(
              leading: const AppBackButton(),
              title: const Text('Giỏ hàng'),
            ),
      body: Obx(() => cartController.getNumOfItemInCart() == 0 ?
        const EmptyCartPage()
          :
        SafeArea(
          child: SingleChildScrollView(
            child: Column(
              children: [
                ...cartController.listCartItem
                    .map((cartItem) => SingleCartItemTile(
                  productId: cartItem['productId'],
                )),
                /*const CouponCodeField(),*/
                const SizedBox(height: 8,),
                const ItemTotalsAndPrice(),
                SizedBox(
                  width: double.infinity,
                  child: Padding(
                    padding: const EdgeInsets.all(AppDefaults.padding),
                    child: ElevatedButton(
                      onPressed: () {
                        // Navigator.pushNamed(context, AppRoutes.orderSuccessfull);
                        Navigator.pushNamed(context, AppRoutes.checkoutPage);
                      },
                      child: const Text('Thanh toán'),
                    ),
                  ),
                ),
              ],
            ),
          ),
      ),)
    );
  }
}
