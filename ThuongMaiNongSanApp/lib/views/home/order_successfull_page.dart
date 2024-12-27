import 'package:apptmns/views/entrypoint/entrypoint_ui.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../core/components/network_image.dart';
import '../../core/constants/app_defaults.dart';

class OrderSuccessfullPage extends StatelessWidget {
  const OrderSuccessfullPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Column(
        children: [
          const Spacer(flex: 2,),
          Padding(
            padding: const EdgeInsets.all(AppDefaults.padding),
            child: SizedBox(
              width: MediaQuery.of(context).size.width * 0.7,
              child: const AspectRatio(
                aspectRatio: 1 / 1,
                child: NetworkImageWithLoader(
                  'https://i.imgur.com/Fj9gVGy.png',
                  fit: BoxFit.contain,
                ),
              ),
            ),
          ),
          Padding(
            padding: const EdgeInsets.all(AppDefaults.padding),
            child: Column(
              children: [
                Text(
                  'Đặt hàng thành công',
                  style: Theme.of(context).textTheme.titleLarge?.copyWith(
                        fontWeight: FontWeight.bold,
                        color: Colors.black,
                      ),
                  textAlign: TextAlign.center,
                ),
                const SizedBox(height: 16),
                const Padding(
                  padding:
                      EdgeInsets.symmetric(horizontal: AppDefaults.padding),
                  child: Text(
                    'Cảm ơn đơn đặt hàng của bạn\n Chúng tôi sẽ cố gắng giao hàng đến bạn sớm nhất',
                    textAlign: TextAlign.center,
                  ),
                ),
              ],
            ),
          ),
          Padding(
            padding: const EdgeInsets.all(AppDefaults.padding),
            child: SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                onPressed: () {
                  /*UiUtil.openDialog(
                          context: context,
                          widget: const DeliverySuccessfullDialog(),
                        );*/
                  Get.offAll(() => const EntryPointUI());
                },
                child: const Text('Tiếp tục mua sắm'),
              ),
            ),
          ),
          const Spacer(),
        ],
      ),
    );
  }
}
