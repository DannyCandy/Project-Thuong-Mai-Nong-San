import 'dart:convert';

import 'package:apptmns/views/auth/dialogs/success_dialogs.dart';
import 'package:apptmns/views/auth/login_page.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';
import 'package:form_field_validator/form_field_validator.dart';
import 'package:loading_animation_widget/loading_animation_widget.dart';
import '../../logic_constants/api_endpoints.dart';

import '../../core/components/app_back_button.dart';
import '../../core/constants/constants.dart';
import '../../core/utils/validators.dart';
import 'dialogs/info_dialogs.dart';
import 'dialogs/no_verified_dialogs.dart';

class PasswordResetPage extends StatefulWidget {
  const PasswordResetPage({super.key});

  @override
  State<PasswordResetPage> createState() => _PasswordResetPageState();
}

class _PasswordResetPageState extends State<PasswordResetPage> {
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  final TextEditingController _oTPController = TextEditingController();
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _newPasswordController = TextEditingController();
  final TextEditingController _confirmPasswordController = TextEditingController();

  bool _isLoading = false;
  bool _isNewPasswordShown = false;
  bool _isConfirmPasswordShown = false;

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

  bool checkFormIsOk(){
    return _formKey.currentState?.validate() ?? false;
  }

  Future<bool> onSubmitResetPassword(bool isFormOkay) async {
    if (isFormOkay) {
      setState(() {
        _isLoading = true;
      });
      final Map<String, String> resetPasswordData = {
        'otp':_oTPController.text,
        'email':_emailController.text,
        'newPassword':_newPasswordController.text,
        'confirmNewPassword':_confirmPasswordController.text,
      };
      final http = ApiEndpoints.makeNoSSLSecureRequest();
      var result = await http.post(
        Uri.parse(ApiEndpoints.resetPassword),
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: jsonEncode(resetPasswordData),
      );

      setState(() {
        _isLoading = false; // Ẩn loading khi kết thúc request
      });

      if (result.statusCode == 200) {
        return true;
      }
    }
    return false;
  }

  @override
  void dispose() {
    _oTPController.dispose();
    _emailController.dispose();
    _newPasswordController.dispose();
    _confirmPasswordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.scaffoldWithBoxBackground,
      appBar: AppBar(
        leading: const AppBackButton(),
        title: const Text('Mật khẩu mới'),
        backgroundColor: AppColors.scaffoldBackground,
      ),
      body: Center(
        child: SingleChildScrollView(
          child: Column(
            children: [
              Container(
                margin: const EdgeInsets.all(AppDefaults.margin),
                padding: const EdgeInsets.symmetric(
                  horizontal: AppDefaults.padding,
                  vertical: AppDefaults.padding * 3,
                ),
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: AppDefaults.borderRadius,
                ),
                child: Form(
                  key: _formKey,
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Text(
                        'Đặt lại mật khẩu',
                        style: Theme.of(context)
                            .textTheme
                            .titleLarge
                            ?.copyWith(fontWeight: FontWeight.bold),
                      ),
                      const SizedBox(height: AppDefaults.padding * 3),
                      const Text("Gửi lại mã OTP"),
                      const SizedBox(height: 8),
                      TextFormField(
                        controller: _oTPController,
                        keyboardType: TextInputType.number,
                        textInputAction: TextInputAction.next,
                      ),
                      const SizedBox(height: AppDefaults.padding),
                      const Text("Email mà bạn đã đăng kí"),
                      const SizedBox(height: 8),
                      TextFormField(
                        controller: _emailController,
                        keyboardType: TextInputType.emailAddress,
                        validator: Validators.registerEmail.call,
                        textInputAction: TextInputAction.next,
                      ),
                      const SizedBox(height: AppDefaults.padding),
                      const Text("Mật khẩu mới"),
                      const SizedBox(height: 8),
                      TextFormField(
                        autofocus: true,
                        textInputAction: TextInputAction.next,
                        controller: _newPasswordController,
                        validator: Validators.password.call,
                        keyboardType: TextInputType.visiblePassword,
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
                      const Text("Xác nhận lại mật khẩu"),
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
                        obscureText: !_isConfirmPasswordShown,
                        textInputAction: TextInputAction.done,
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
                      const SizedBox(height: AppDefaults.padding * 2),
                      SizedBox(
                        width: double.infinity,
                        child: _isLoading ? Center(child: LoadingAnimationWidget.staggeredDotsWave(color: AppColors.primary, size: 150))
                        :
                        ElevatedButton(
                          onPressed: () async {
                            bool isOk = checkFormIsOk();
                            if(isOk){
                              bool isRequestSuccess = await onSubmitResetPassword(isOk);
                              if(!context.mounted) return;
                              if(isRequestSuccess){
                                showGeneralDialog(
                                  barrierLabel: 'DialogInfo',
                                  barrierDismissible: false,
                                  context: context,
                                  pageBuilder: (ctx, anim1, anim2) => SuccessDialog(
                                      onPressed: () => Navigator.pushAndRemoveUntil(
                                        context,
                                        CupertinoPageRoute(builder: (_) => const LoginPage()),
                                            (Route<dynamic> route) => false,
                                      )
                                      , message: 'Cập nhật mật khẩu thành công.', buttonTitle: 'Đăng nhập'),
                                  transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
                                    scale: anim1,
                                    child: child,
                                  ),
                                );
                              }else{
                                showGeneralDialog(
                                  barrierLabel: 'DialogError',
                                  barrierDismissible: true,
                                  context: context,
                                  pageBuilder: (ctx, anim1, anim2) => const NoVerifiedDialogs(message: 'Xảy ra lỗi trong quá trình lấy lại mật khẩu.'),
                                  transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
                                    scale: anim1,
                                    child: child,
                                  ),
                                );
                              }
                            }
                            return;
                          },
                          child: const Text('Hoàn thành'),
                        ),
                      ),
                    ],
                  ),
                ),
              )
            ],
          ),
        ),
      ),
    );
  }
}
