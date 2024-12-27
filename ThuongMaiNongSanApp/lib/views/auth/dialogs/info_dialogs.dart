import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../core/components/network_image.dart';
import '../../../core/constants/app_defaults.dart';
import '../../../core/constants/app_images.dart';

class InfoDialog extends StatelessWidget {
  final String buttonTitle;
  final String message;
  const InfoDialog({super.key, required this.onPressed, required this.message, required this.buttonTitle});

  final void Function() onPressed;
  @override
  Widget build(BuildContext context) {
    return Dialog(
      shape: RoundedRectangleBorder(borderRadius: AppDefaults.borderRadius),
      child: Padding(
        padding: const EdgeInsets.symmetric(
          vertical: AppDefaults.padding * 3,
          horizontal: AppDefaults.padding,
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            SizedBox(
              width: MediaQuery.of(context).size.width * 0.35,
              child: const AspectRatio(
                aspectRatio: 1 / 1,
                child: NetworkImageWithLoader(
                  AppImages.verified,
                  fit: BoxFit.contain,
                ),
              ),
            ),
            const SizedBox(height: AppDefaults.padding),
            Text(
              'Thông tin',
              style: Theme.of(context)
                  .textTheme
                  .headlineSmall
                  ?.copyWith(fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: AppDefaults.padding),
            Text(message,textAlign: TextAlign.center,),
            const SizedBox(height: AppDefaults.padding),
            Row(
              children: [
                const SizedBox(width: AppDefaults.padding),
                Expanded(
                  /*width: double.infinity,*/
                  child: ElevatedButton(
                    onPressed: onPressed,
                    child: Text(buttonTitle),
                  ),
                ),
                const SizedBox(width: AppDefaults.padding),
                Expanded(
                  /*width: double.infinity,*/
                  child: ElevatedButton(
                    onPressed: () {
                      Get.back();
                    },
                    child: const Text('Trở về'),
                  ),
                ),
                const SizedBox(width: AppDefaults.padding),
              ],
            )
          ],
        ),
      ),
    );
  }
}
