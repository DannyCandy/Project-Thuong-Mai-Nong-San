import 'package:apptmns/views/auth/intro_login_page.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../controllers/user_controller.dart';
import '../../core/constants/app_colors.dart';
import 'components/profile_header.dart';
import 'components/profile_menu_options.dart';

class ProfilePage extends StatelessWidget {
  const ProfilePage({super.key});
  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => UserController());
    final userController = Get.find<UserController>();

    /*userController.getAllUserInfo();*/

    // Gọi getAllUserInfo sau khi build xong
    WidgetsBinding.instance.addPostFrameCallback((_) {
      userController.getAllUserInfo();
    });

    return Obx((){
      if(!userController.isLoading.value){
        return SingleChildScrollView(
          child: Container(
            color: AppColors.cardColor,
            child: const Column(
              children: [
                ProfileHeader(),
                ProfileMenuOptions(),
              ],
            ),
          ),
        );
      }
      return const IntroLoginPage();
    });
  }
}
