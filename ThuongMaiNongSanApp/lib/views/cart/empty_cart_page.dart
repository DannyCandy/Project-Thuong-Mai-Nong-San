import 'package:apptmns/views/entrypoint/entrypoint_ui.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../core/components/network_image.dart';
import '../../core/constants/app_defaults.dart';

class EmptyCartPage extends StatelessWidget {
  const EmptyCartPage({super.key});

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: MediaQuery.of(context).size.width,
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          const Spacer(flex: 2),
          SizedBox(
            width: MediaQuery.of(context).size.width * 0.7,
            child: const Padding(
              padding: EdgeInsets.all(AppDefaults.padding * 2),
              child: AspectRatio(
                aspectRatio: 1 / 1,
                child:
                    NetworkImageWithLoader('https://i.imgur.com/3avdket.png'),
              ),
            ),
          ),
          Text(
            'Oppss!',
            style: Theme.of(context)
                .textTheme
                .titleLarge
                ?.copyWith(fontWeight: FontWeight.bold, color: Colors.black),
          ),
          const SizedBox(height: 8),
          const Text('Không có sản phẩm nào trong giỏ hàng'),
          const Spacer(),
          SizedBox(
            width: double.infinity,
            child: Padding(
              padding: const EdgeInsets.all(AppDefaults.padding * 2),
              child: ElevatedButton(
                onPressed: () {
                  Get.offAll(() => const EntryPointUI());
                },
                child: const Text('Về trang chủ'),
              ),
            ),
          ),
          const Spacer(),
        ],
      ),
    );
  }
}
