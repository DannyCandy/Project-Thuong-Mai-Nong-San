
import 'package:apptmns/controllers/user_controller.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';
import 'package:form_field_validator/form_field_validator.dart';
import 'package:get/get.dart';
import 'package:loading_animation_widget/loading_animation_widget.dart';
import '../../../core/components/app_back_button.dart';
import '../../../core/constants/constants.dart';
import '../../../core/utils/validators.dart';
import '../../../services/user_info_store.dart';

class ChangePasswordPage extends StatefulWidget {
  const ChangePasswordPage({super.key});

  @override
  State<ChangePasswordPage> createState() => _ChangePasswordPageState();
}

class _ChangePasswordPageState extends State<ChangePasswordPage> {
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  final TextEditingController _currentPasswordController = TextEditingController();
  final TextEditingController _newPasswordController = TextEditingController();
  final TextEditingController _confirmPasswordController = TextEditingController();

  final String userID = UserPreferences.getUserId()!;
  bool _isCurrentPasswordShown = false;
  bool _isNewPasswordShown = false;
  bool _isConfirmPasswordShown = false;

  final UserController userController = Get.put(UserController());

  onCurrentPassShowClicked() {
    setState(() {
      _isCurrentPasswordShown = !_isCurrentPasswordShown;
    });
  }

  onNewPassShowClicked() {
    setState(() {
      _isNewPasswordShown = !_isNewPasswordShown;
    });
  }

  onConfirmPassShowClicked() {
    setState(() {
      _isConfirmPasswordShown = !_isConfirmPasswordShown;
    });
  }

  @override
  void dispose() {
    _currentPasswordController.dispose();
    _newPasswordController.dispose();
    _confirmPasswordController.dispose();
    super.dispose();
  }

  bool checkFormIsOk(){
    return _formKey.currentState?.validate() ?? false;
  }

  Future<void> onSubmitChangePassword(bool isFormOkay) async {
    if (isFormOkay) {
      final Map<String, String> changePasswordData = {
        'userId': userID,
        'currentPassword': _currentPasswordController.text,
        'newPassword': _newPasswordController.text,
        'confirmNewPassword': _confirmPasswordController.text,
      };
      await userController.changeUserPassword(changePasswordData);
      /*final http = ApiEndpoints.makeNoSSLSecureRequest();
      var result = await http.post(
        Uri.parse(ApiEndpoints.changePassword),
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          'Authorization': 'Bearer ${UserPreferences.getUserToken()}',
        },
        body: jsonEncode(changePasswordData),
      );*/
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: const AppBackButton(),
        title: const Text(
          'Thay đổi mật khẩu',
        ),
      ),
      backgroundColor: AppColors.cardColor,
      body: Center(
        child: SingleChildScrollView(
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
            child: Form(
              key: _formKey,
              child: Column(
                mainAxisSize: MainAxisSize.min,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  /* <----  Current Password -----> */
                  const Text("Mật khẩu hiện tại"),
                  const SizedBox(height: 8),
                  TextFormField(
                    controller: _currentPasswordController,
                    validator: Validators.password.call,
                    keyboardType: TextInputType.visiblePassword,
                    textInputAction: TextInputAction.next,
                    obscureText: !_isCurrentPasswordShown,
                    decoration: InputDecoration(
                      suffixIcon: Material(
                        color: Colors.transparent,
                        child: IconButton(
                          onPressed: onCurrentPassShowClicked,
                          icon: SvgPicture.asset(
                            AppIcons.eye,
                            width: 24,
                          ),
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(height: AppDefaults.padding),

                  /* <---- New Password -----> */
                  const Text("Mật khẩu mới"),
                  const SizedBox(height: 8),
                  TextFormField(
                    controller: _newPasswordController,
                    validator: Validators.password.call,
                    keyboardType: TextInputType.visiblePassword,
                    textInputAction: TextInputAction.next,
                    obscureText: !_isNewPasswordShown,
                    decoration: InputDecoration(
                      suffixIcon: Material(
                        color: Colors.transparent,
                        child: IconButton(
                          onPressed: onNewPassShowClicked,
                          icon: SvgPicture.asset(
                            AppIcons.eye,
                            width: 24,
                          ),
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(height: AppDefaults.padding),

                  /* <---- Confirm Password-----> */
                  const Text("Xác nhận mật khẩu"),
                  const SizedBox(height: 8),
                  TextFormField(
                    controller: _confirmPasswordController,
                    validator: (value){
                      String? validText = Validators.password.call(value);
                      if(validText != null){
                        return validText;
                      }
                      return MatchValidator(errorText: 'Mật khẩu không khớp').validateMatch(value!, _newPasswordController.text);
                    },
                    keyboardType: TextInputType.visiblePassword,
                    textInputAction: TextInputAction.done,
                    obscureText: !_isConfirmPasswordShown,
                    decoration: InputDecoration(
                      suffixIcon: Material(
                        color: Colors.transparent,
                        child: IconButton(
                          onPressed: onConfirmPassShowClicked,
                          icon: SvgPicture.asset(
                            AppIcons.eye,
                            width: 24,
                          ),
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(height: AppDefaults.padding),

                  /* <---- Submit -----> */
                  const SizedBox(height: AppDefaults.padding),
                  SizedBox(
                    width: double.infinity,
                    child: Obx(() => userController.isLoading.value ?
                    Center(child: LoadingAnimationWidget.staggeredDotsWave(color: AppColors.primary, size: 150))
                        :
                    ElevatedButton(
                      child: const Text('Cập nhật'),
                      onPressed: () async {
                        bool isOk = checkFormIsOk();
                        if(isOk) {
                          await onSubmitChangePassword(isOk);
                        }
                        _currentPasswordController.clear();
                        _newPasswordController.clear();
                        _confirmPasswordController.clear();
                      },
                    ),)
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
