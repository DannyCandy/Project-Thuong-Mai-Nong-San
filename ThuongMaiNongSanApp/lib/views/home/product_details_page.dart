import 'package:apptmns/controllers/product_controller.dart';
import 'package:apptmns/logic_constants/api_endpoints.dart';
import 'package:apptmns/views/cart/cart_page.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../controllers/cart_controller.dart';
import '../../core/components/app_back_button.dart';
import '../../core/components/buy_now_row_button.dart';
import '../../core/components/price_and_quantity.dart';
import '../../core/components/product_images_slider.dart';
import '../../core/constants/app_defaults.dart';
import '../auth/dialogs/success_dialogs.dart';

class ProductDetailsPage extends StatelessWidget {
  const ProductDetailsPage({super.key,required this.productId});

  final String productId;
  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => ProductController());
    final productController = Get.find<ProductController>();

    Get.lazyPut(() => CartController());
    final cartController = Get.find<CartController>();

    cartController.setQuantityOfCurrentItem(1);

    return FutureBuilder<void>(
      future: productController.getSingleProducts(productId),
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.waiting) {
          return const Center(child: CircularProgressIndicator());
        } else if (snapshot.hasError) {
          return Center(child: Text('Lỗi: ${snapshot.error}'));
        } else {
          return Scaffold(
            resizeToAvoidBottomInset: false,
            appBar: AppBar(
              leading: const AppBackButton(),
              title: const Text('Chi tiết sản phẩm'),
            ),
            bottomNavigationBar: SafeArea(
              child: Padding(
                padding: const EdgeInsets.symmetric(horizontal: AppDefaults.padding),
                child: BuyNowRow(
                  onBuyButtonTap: () {
                    cartController.addToCart(productId,
                        productController.singleProductItem.value.spName!,
                        productController.singleProductItem.value.hinhAnhSp!,
                        cartController.getQuantityOfCurrentItem(),
                        productController.singleProductItem.value.price!);
                    Get.to(() => const CartPage());
                  },
                  onCartButtonTap: () {
                    cartController.addToCart(productId,
                        productController.singleProductItem.value.spName!,
                        productController.singleProductItem.value.hinhAnhSp!,
                        cartController.getQuantityOfCurrentItem(),
                        productController.singleProductItem.value.price!);
                    Get.dialog(
                      SuccessDialog(
                        onPressed: () => Get.back(),
                        message: 'Thêm sản phẩm vào giỏ hàng thành công.',
                        buttonTitle: 'OK',
                      ),
                    );
                  },
                ),
              ),
            ),
            body: SingleChildScrollView(
              child: Column(
                children: [
                  Obx(() =>
                      ProductImagesSlider(
                        images: [
                          '${ApiEndpoints.apiUri}${productController.singleProductItem.value.hinhAnhSp}',
                        ],
                        productId: productId,
                      )),
                  SizedBox(
                    width: double.infinity,
                    child: Padding(
                        padding: const EdgeInsets.all(AppDefaults.padding),
                        child: Obx(() =>
                            Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Text(
                                  productController.singleProductItem.value.spName??'***',
                                  style: Theme.of(context).textTheme.titleLarge?.copyWith(
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                                const SizedBox(height: 8),
                                Text('Phân loại: ${productController.singleProductItem.value.categoryName??'***'}'),
                              ],
                            ),)
                    ),
                  ),
                  Padding(
                      padding: const EdgeInsets.symmetric(horizontal: AppDefaults.padding),
                      child: Obx(() =>
                          PriceAndQuantityRow(
                            currentPrice: productController.singleProductItem.value.price??0,
                            quantity: 1,
                          ),
                      )
                  ),
                  const SizedBox(height: 8),

                  /// Product Details
                  Obx(() =>
                      Padding(
                        padding: const EdgeInsets.all(AppDefaults.padding),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              'Chi tiết',
                              style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                                fontWeight: FontWeight.bold,
                                color: Colors.black,
                              ),
                            ),
                            const SizedBox(height: 8),
                            Text(
                                productController.singleProductItem.value.moTa??'Lỗi mô tả'
                            ),
                            Text(
                                productController.singleProductItem.value.thanhPhanDinhDuong??'Lỗi thành phần dinh dưỡng'
                            ),
                          ],
                        ),
                      ),
                  ),

                  /// Review Row
                  const Padding(
                    padding: EdgeInsets.symmetric(
                      horizontal: AppDefaults.padding,
                      // vertical: AppDefaults.padding,
                    ),
                    child: Column(
                      children: [
                        Divider(thickness: 0.1),
                        /*ReviewRowButton(totalStars: 5),*/
                        Divider(thickness: 0.1),
                      ],
                    ),
                  ),
                ],
              ),
            ),
          );
        }
      },
    );
    /*return Scaffold(
      resizeToAvoidBottomInset: false,
      appBar: AppBar(
        leading: const AppBackButton(),
        title: const Text('Chi tiết sản phẩm'),
      ),
      bottomNavigationBar: SafeArea(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: AppDefaults.padding),
          child: BuyNowRow(
            onBuyButtonTap: () {
              cartController.addToCart(productId,
                  productController.singleProductItem.value.spName!,
                  productController.singleProductItem.value.hinhAnhSp!,
                  cartController.getQuantityOfCurrentItem(),
                  productController.singleProductItem.value.price!);
              Get.to(() => const CartPage());
            },
            onCartButtonTap: () {
              cartController.addToCart(productId,
                  productController.singleProductItem.value.spName!,
                  productController.singleProductItem.value.hinhAnhSp!,
                  cartController.getQuantityOfCurrentItem(),
                  productController.singleProductItem.value.price!);
              Get.dialog(
                SuccessDialog(
                  onPressed: () => Get.back(),
                  message: 'Thêm sản phẩm vào giỏ hàng thành công.',
                  buttonTitle: 'OK',
                ),
              );
            },
          ),
        ),
      ),
      body: SingleChildScrollView(
        child: Column(
          children: [
            Obx(() =>
              ProductImagesSlider(
              images: [
                '${ApiEndpoints.apiUri}${productController.singleProductItem.value.hinhAnhSp}',
              ],
            )),
            SizedBox(
              width: double.infinity,
              child: Padding(
                padding: const EdgeInsets.all(AppDefaults.padding),
                child: Obx(() =>
                    Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          productController.singleProductItem.value.spName??'***',
                          style: Theme.of(context).textTheme.titleLarge?.copyWith(
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                        const SizedBox(height: 8),
                        Text('Phân loại: ${productController.singleProductItem.value.categoryName??'***'}'),
                      ],
                    ),)
              ),
            ),
            Padding(
              padding: const EdgeInsets.symmetric(horizontal: AppDefaults.padding),
              child: Obx(() =>
                  PriceAndQuantityRow(
                    currentPrice: productController.singleProductItem.value.price??0,
                    quantity: 2,
                  ),
              )
            ),
            const SizedBox(height: 8),

            /// Product Details
            Obx(() =>
                Padding(
                  padding: const EdgeInsets.all(AppDefaults.padding),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        'Chi tiết',
                        style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                          fontWeight: FontWeight.bold,
                          color: Colors.black,
                        ),
                      ),
                      const SizedBox(height: 8),
                      Text(
                        productController.singleProductItem.value.moTa??'Lỗi mô tả'
                      ),
                      Text(
                        productController.singleProductItem.value.thanhPhanDinhDuong??'Lỗi thành phần dinh dưỡng'
                      ),
                    ],
                  ),
                ),
            ),

            /// Review Row
            const Padding(
              padding: EdgeInsets.symmetric(
                horizontal: AppDefaults.padding,
                // vertical: AppDefaults.padding,
              ),
              child: Column(
                children: [
                  Divider(thickness: 0.1),
                  ReviewRowButton(totalStars: 5),
                  Divider(thickness: 0.1),
                ],
              ),
            ),
          ],
        ),
      ),
    );*/
  }
}
