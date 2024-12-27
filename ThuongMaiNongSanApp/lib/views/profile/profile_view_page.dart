
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../controllers/user_controller.dart';
import '../../core/components/app_back_button.dart';
import '../../core/constants/constants.dart';

class ProfileViewPage extends StatelessWidget {
  const ProfileViewPage({super.key});

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => UserController());
    final userController = Get.find<UserController>(); // Lấy UserController

    return Scaffold(
      backgroundColor: AppColors.cardColor,
      appBar: AppBar(
        leading: const AppBackButton(),
        title: const Text(
          'Thông tin cá nhân của tôi',
        ),
      ),
      body: SingleChildScrollView(
        child: Container(
          margin: const EdgeInsets.all(AppDefaults.padding),
          padding: const EdgeInsets.symmetric(
            horizontal: AppDefaults.padding,
            vertical: AppDefaults.padding * 2,
          ),
          decoration: BoxDecoration(
            color: AppColors.scaffoldBackground,
            borderRadius: AppDefaults.borderRadius,
          ),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              /* <----  First Name -----> */
              const Text("Tên khách hàng"),
              const SizedBox(height: 8),
              Obx(() => TextFormField(
                readOnly: true,
                initialValue: userController.listUserInfo['tenKhachHang'],
              )),
              const SizedBox(height: AppDefaults.padding),

              /* <---- Last Name -----> */
              const Text("Tên đăng nhập"),
              const SizedBox(height: 8),
              Obx(() => TextFormField(
                readOnly: true,
                initialValue: userController.listUserInfo['userName'],
              )),
              const SizedBox(height: AppDefaults.padding),

              /* <---- Phone Number -----> */
              const Text("Số điện thoại"),
              const SizedBox(height: 8),
              Obx(() => TextFormField(
                readOnly: true,
                initialValue: userController.listUserInfo['phone'],
              )),
              const SizedBox(height: AppDefaults.padding),

              /* <---- Gender -----> */
              const Text("Địa chỉ"),
              const SizedBox(height: 8),
              Obx(() => TextFormField(
                readOnly: true,
                initialValue: userController.listUserInfo['diaChi'],
              )),
              const SizedBox(height: AppDefaults.padding),
              const SizedBox(height: AppDefaults.padding),
            ],
          ),
        ),
      ),
    );
  }
}
