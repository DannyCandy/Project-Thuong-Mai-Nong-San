import 'dart:convert';

import 'package:apptmns/views/auth/dialogs/success_dialogs.dart';
import 'package:flutter/material.dart';
import 'package:loading_animation_widget/loading_animation_widget.dart';

import '../../logic_constants/api_endpoints.dart';
import '../../core/components/app_back_button.dart';
import '../../core/constants/app_colors.dart';
import '../../core/constants/app_defaults.dart';
import '../../core/routes/app_routes.dart';
import '../../core/utils/validators.dart';
import 'dialogs/info_dialogs.dart';
import 'dialogs/no_verified_dialogs.dart';

class ForgetPasswordPage extends StatefulWidget {
  const ForgetPasswordPage({super.key});

  @override
  State<ForgetPasswordPage> createState() => _ForgetPasswordState();
}

class _ForgetPasswordState extends State<ForgetPasswordPage> {
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  final TextEditingController _emailController = TextEditingController();

  bool _isLoading = false;

  @override
  void dispose() {
    _emailController.dispose();
    super.dispose();
  }
  bool checkFormIsOk(){
    return _formKey.currentState?.validate() ?? false;
  }
  Future<bool> onSubmitSendOTPResetPassword(bool isFormOkay) async {
    if (isFormOkay) {
      setState(() {
        _isLoading = true;
      });
      final Map<String, String> sendOtpResetPasswordData = {
        'email':_emailController.text
      };
      final http = ApiEndpoints.makeNoSSLSecureRequest();
      var result = await http.post(
        Uri.parse(ApiEndpoints.sendOTPResetPassword),
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: jsonEncode(sendOtpResetPasswordData),
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
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.scaffoldWithBoxBackground,
      appBar: AppBar(
        leading: const AppBackButton(),
        title: const Text('Quên mật khẩu'),
        backgroundColor: AppColors.scaffoldBackground,
      ),
      body: Center(
        child: SingleChildScrollView(
          child: Form(
            key: _formKey,
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
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Text(
                        'Lấy lại mật khẩu',
                        style: Theme.of(context)
                            .textTheme
                            .titleLarge
                            ?.copyWith(fontWeight: FontWeight.bold),
                      ),
                      const SizedBox(height: AppDefaults.padding),
                      const Text(
                        'Hãy nhập email để chúng tôi gửi mã xác nhận đến email của bạn.',
                      ),
                      const SizedBox(height: AppDefaults.padding * 3),
                      const Text("Địa chỉ email"),
                      const SizedBox(height: 8),
                      TextFormField(
                        controller: _emailController,
                        autofocus: true,
                        textInputAction: TextInputAction.next,
                        validator: Validators.registerEmail.call,
                        keyboardType: TextInputType.emailAddress,
                      ),
                      const SizedBox(height: AppDefaults.padding),
                      const SizedBox(height: AppDefaults.padding),
                      SizedBox(
                        width: double.infinity,
                        child: _isLoading ? Center(child: LoadingAnimationWidget.staggeredDotsWave(color: AppColors.primary, size: 150))
                        :
                        ElevatedButton(
                          onPressed: () async {
                            bool isOk = checkFormIsOk();
                            if(isOk){
                              bool isRequestSuccess = await onSubmitSendOTPResetPassword(isOk);
                              if(!context.mounted) return;
                              if(isRequestSuccess){
                                showGeneralDialog(
                                  barrierLabel: 'Thông tin',
                                  barrierDismissible: false,
                                  context: context,
                                  pageBuilder: (ctx, anim1, anim2) => SuccessDialog(onPressed: () => Navigator.pushNamed(context, AppRoutes.passwordReset),
                                      message: 'Chúng tôi đã gửi OTP đến email đăng kí của bạn\nHãy sử dụng OTP đó để lấy lại mật khẩu',
                                      buttonTitle: 'Ok'),
                                  transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
                                    scale: anim1,
                                    child: child,
                                  ),
                                );
                              }else{
                                showGeneralDialog(
                                  barrierLabel: 'Lỗi',
                                  barrierDismissible: true,
                                  context: context,
                                  pageBuilder: (ctx, anim1, anim2) => const NoVerifiedDialogs(message: 'Người dùng cần đăng nhập trên app ít nhất\n1 lần để thưc hiện thao tác này.'),
                                  transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
                                    scale: anim1,
                                    child: child,
                                  ),
                                );
                              }
                            }
                            return;
                          },
                          child: const Text('Gửi mã cho tôi'),
                        ),
                      ),
                    ],
                  ),
                )
              ],
            ),
          ),
        ),
      ),
    );
  }
}
