import 'package:apptmns/controllers/address_controller.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../core/components/title_and_action_button.dart';
import '../../../core/routes/app_routes.dart';
import 'checkout_address_card.dart';

class AddressSelector extends StatelessWidget {
  const AddressSelector({
    super.key,
  });

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => AddressController());
    final addressController = Get.find<AddressController>();

    return Column(
      children: [
        TitleAndActionButton(
          title: 'Chọn địa chỉ giao hàng',
          actionLabel: 'Thêm mới',
          onTap: () {
            Navigator.pushNamed(context, AppRoutes.newAddress);
          },
          isHeadline: false,
        ),
        Obx(() {
          /*return SingleChildScrollView(
            child: Column(
              children: addressController.listUserAddress.map((item) {
                return AddressCard(
                  label: item['defaultPos'] ? 'Mặc định' : '',
                  phoneNumber: item['phone'] ?? 'No Phone Number',
                  address: item['dcgiaoHang'] ?? 'No Address',
                  isActive: item['isChosen'] ?? false,
                  onTap: () {
                    addressController.setChosen(item['idttlh']);
                  },
                );
              }).toList(),
            ),
          );*/
          return SizedBox(
            height: MediaQuery.of(context).size.height * 0.4, // 40% chiều cao màn hình
            child: SingleChildScrollView(
              child: Column(
                children: addressController.listUserAddress.map((item) {
                  return AddressCard(
                    label: item['defaultPos'] ? 'Mặc định' : '',
                    phoneNumber: item['phone'] ?? 'Không có số điện thoại',
                    address: item['dcgiaoHang'] ?? 'Không có địa chỉ',
                    isActive: item['isChosen'] ?? false,
                    onTap: () {
                      addressController.setChosen(item['idttlh']);
                    },
                  );
                }).toList(),
              ),
            ),
          );
        }),
      ],
    );
  }
}
