import 'package:apptmns/controllers/order_history_controller.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../core/components/app_back_button.dart';
import '../../../core/constants/app_colors.dart';
import 'components/custom_tab_label.dart';
import 'components/tab_all.dart';

class AllOrderPage extends StatelessWidget {
  const AllOrderPage({super.key});

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => OrderHistoryController());

    return DefaultTabController(
      length: 1,
      child: Scaffold(
        appBar: AppBar(
          leading: const AppBackButton(),
          title: const Text('Đơn hàng của tôi'),
          bottom: const TabBar(
            physics: NeverScrollableScrollPhysics(),
            tabs: [
              CustomTabLabel(label: 'Tất cả', value: ''),
              /*CustomTabLabel(label: 'Running', value: '(14)'),
              CustomTabLabel(label: 'Previous', value: '(44)'),*/
            ],
          ),
        ),
        body: Container(
          color: AppColors.cardColor,
          child: const TabBarView(
            children: [
              AllTab(),
              /*RunningTab(),
              CompletedTab(),*/
            ],
          ),
        ),
      ),
    );
  }
}
