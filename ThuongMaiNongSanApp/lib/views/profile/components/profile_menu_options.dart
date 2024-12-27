import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:loading_animation_widget/loading_animation_widget.dart';

import '../../../controllers/user_controller.dart';
import '../../../core/constants/constants.dart';
import '../../../core/routes/app_routes.dart';
import 'profile_list_tile.dart';

class ProfileMenuOptions extends StatelessWidget {
  const ProfileMenuOptions({
    super.key,
  });

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => UserController());
    final userController = Get.find<UserController>();

    return Container(
      margin: const EdgeInsets.all(AppDefaults.padding),
      padding: const EdgeInsets.all(AppDefaults.padding),
      decoration: BoxDecoration(
        color: Colors.white,
        boxShadow: AppDefaults.boxShadow,
        borderRadius: AppDefaults.borderRadius,
      ),
      child: Column(
        children: [
          ProfileListTile(
            title: 'Xem thông tin của tôi',
            icon: AppIcons.profilePerson,
            onTap: () => Navigator.pushNamed(context, AppRoutes.profileViewer),
          ),
          const Divider(thickness: 0.1),
          /*ProfileListTile(
            title: 'Notification',
            icon: AppIcons.profileNotification,
            onTap: () => Navigator.pushNamed(context, AppRoutes.notifications),
          ),
          const Divider(thickness: 0.1),*/
          ProfileListTile(
            title: 'Cài đặt',
            icon: AppIcons.profileSetting,
            onTap: () => Navigator.pushNamed(context, AppRoutes.settings),
          ),
          const Divider(thickness: 0.1),
          /*ProfileListTile(
            title: 'Payment',
            icon: AppIcons.profilePayment,
            onTap: () => Navigator.pushNamed(context, AppRoutes.paymentMethod),
          ),
          const Divider(thickness: 0.1),*/
          userController.isLoading.value ?
          Center(child: LoadingAnimationWidget.staggeredDotsWave(color: AppColors.primary, size: 50))
            :
          ProfileListTile(
              title: 'Thoát',
              icon: AppIcons.profileLogout,
              onTap: () async {
                await userController.logout();
              }
          ),
        ],
      ),
    );
  }
}
