import 'package:flutter/material.dart';

import '../../core/components/app_back_button.dart';
import '../../core/constants/app_defaults.dart';

class AboutUsPage extends StatelessWidget {
  const AboutUsPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: const AppBackButton(),
        title: const Text('Về chúng tôi'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(AppDefaults.padding),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Về chúng tôi',
              style: Theme.of(context).textTheme.titleLarge?.copyWith(
                    fontWeight: FontWeight.bold,
                  ),
            ),
            const SizedBox(height: AppDefaults.padding),
            const Text(
              'Ứng dụng của chúng tôi mang đến sự tiện lợi trong việc mua sắm tạp hóa, '
              'đặc biệt với rau củ quả tươi, gạo sạch và các nhu yếu phẩm hàng ngày.\n\n'
              'Với giao diện thân thiện, dễ sử dụng, khách hàng có thể tìm kiếm và đặt hàng nhanh chóng.'
              'Chúng tôi cam kết mang lại sản phẩm chất lượng cao, nguồn gốc rõ ràng và giao hàng tận nơi,'
              'giúp tiết kiệm thời gian và đảm bảo sức khỏe cho gia đình bạn.\n\n'
              'Mua sắm chưa bao giờ dễ dàng đến thế, hãy trải nghiệm ngay hôm nay!'
            )
          ],
        ),
      ),
    );
  }
}
