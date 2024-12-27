import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';

import '../../../core/components/app_back_button.dart';
import '../../../core/constants/constants.dart';
import '../../../core/routes/app_routes.dart';
import '../../../core/components/app_settings_tile.dart';

class SettingsPage extends StatelessWidget {
  const SettingsPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: const AppBackButton(),
        title: const Text(
          'Cài đặt',
        ),
      ),
      backgroundColor: AppColors.cardColor,
      body: Container(
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
          children: [
            /*AppSettingsListTile(
              label: 'Language',
              trailing: SvgPicture.asset(AppIcons.right),
              onTap: () =>
                  Navigator.pushNamed(context, AppRoutes.settingsLanguage),
            ),*/
            /*AppSettingsListTile(
              label: 'Notification',
              trailing: SvgPicture.asset(AppIcons.right),
              onTap: () =>
                  Navigator.pushNamed(context, AppRoutes.settingsNotifications),
            ),*/
            AppSettingsListTile(
              label: 'Đổi mật khẩu',
              trailing: SvgPicture.asset(AppIcons.right),
              onTap: () =>
                  Navigator.pushNamed(context, AppRoutes.changePassword),
            ),
            AppSettingsListTile(
              label: 'Đổi email',
              trailing: SvgPicture.asset(AppIcons.right),
              onTap: () =>
                  Navigator.pushNamed(context, AppRoutes.changeEmail),
            ),
            AppSettingsListTile(
              label: 'Đổi avatar',
              trailing: SvgPicture.asset(AppIcons.right),
              onTap: () =>
                  Navigator.pushNamed(context, AppRoutes.changeAvatar),
            ),
            AppSettingsListTile(
              label: 'Cập nhật địa chỉ giao hàng',
              trailing: SvgPicture.asset(AppIcons.right),
              onTap: () =>
                  Navigator.pushNamed(context, AppRoutes.deliveryAddress),
            ),
            /*AppSettingsListTile(
              label: 'Location',
              trailing: SvgPicture.asset(AppIcons.right),
              onTap: () {},
            ),*/
            AppSettingsListTile(
              label: 'Cập nhật thông tin',
              trailing: SvgPicture.asset(AppIcons.right),
              onTap: () =>
                  Navigator.pushNamed(context, AppRoutes.profileEdit),
            ),
            /*AppSettingsListTile(
              label: 'Deactivate Account',
              trailing: SvgPicture.asset(AppIcons.right),
              onTap: () =>
                  Navigator.pushNamed(context, AppRoutes.introLogin),
            ),*/
          ],
        ),
      ),
    );
  }
}
