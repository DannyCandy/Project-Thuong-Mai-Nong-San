import 'package:apptmns/controllers/address_controller.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:loading_animation_widget/loading_animation_widget.dart';

import '../../controllers/cart_controller.dart';
import '../../core/components/app_back_button.dart';
import '../../core/constants/app_colors.dart';
import '../../core/constants/app_defaults.dart';
import 'components/checkout_address_selector.dart';
import 'components/checkout_payment_systems.dart';

class CheckoutPage extends StatelessWidget {
  const CheckoutPage({super.key});
  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => AddressController());
    final addressController = Get.find<AddressController>();
    Get.lazyPut(() => CartController());
    final cartController = Get.find<CartController>();

    return Scaffold(
      appBar: AppBar(
        leading: const AppBackButton(),
        title: const Text('Thanh toán'),
      ),
      body:SingleChildScrollView(
        child: Column(
          children: [
            const AddressSelector(),
            const PaymentSystem(),
            /*const CardDetails(),*/
            Padding(
              padding: const EdgeInsets.all(AppDefaults.padding),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                // Name Field
                const Text("Lời nhắn"),
                const SizedBox(height: 8),
                Obx(() =>
                    TextFormField(
                      controller: cartController.textFormField.value,
                      keyboardType: TextInputType.text,
                      textInputAction: TextInputAction.next,
                    ),)]
              )
            ),

            Obx(() => cartController.isLoading.value ?
              LoadingAnimationWidget.staggeredDotsWave(color: AppColors.primary, size: 150)
              :
              PayNowButton(addressId: addressController.getChosenUserAddressId())),
            const SizedBox(height: 16),
          ],
        ),
      ),
    );
  }
}

class PayNowButton extends StatelessWidget {
  const PayNowButton({
    super.key,
    required this.addressId,
  });

  final String addressId;

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => CartController());
    final cartController = Get.find<CartController>();

    return SizedBox(
      width: double.infinity,
      child: Padding(
        padding: const EdgeInsets.all(AppDefaults.padding),
        child:
          ElevatedButton(
            onPressed: () async {
              await cartController.checkOut(addressId);
            },
            child: const Text('Thanh toán ngay'),
          ),
      ),
    );
  }
}
