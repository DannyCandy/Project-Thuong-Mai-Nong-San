import 'package:flutter/material.dart';

import '../../../core/components/network_image.dart';
import '../../../core/constants/app_defaults.dart';
import '../../../core/constants/app_images.dart';

class WarningDialogs extends StatelessWidget {
  final String message;
  final Future<dynamic> Function() onAgree;
  const WarningDialogs({super.key, required this.message, required this.onAgree});

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      shape: RoundedRectangleBorder(borderRadius: AppDefaults.borderRadius),
      content: Padding(
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
                  AppImages.error,
                  fit: BoxFit.contain,
                ),
              ),
            ),
            const SizedBox(height: AppDefaults.padding),
            Text(
              'Lưu ý',
              style: Theme.of(context)
                  .textTheme
                  .headlineSmall
                  ?.copyWith(fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: AppDefaults.padding),
            Text(
              message,
              textAlign: TextAlign.center,
            ),
            const SizedBox(height: AppDefaults.padding),
            Row(
              children: [
                Expanded(
                  child: ElevatedButton(
                    onPressed: () async {
                      Navigator.of(context).pop();
                      await onAgree(); // Gọi hàm xử lý khi người dùng đồng ý
                    },
                    child: const Text('Đồng ý'),
                  ),
                ),
                const SizedBox(width: AppDefaults.padding),
                Expanded(
                  child: ElevatedButton(
                    onPressed: () {
                      Navigator.of(context).pop();
                    },
                    child: const Text('Huỷ'),
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
