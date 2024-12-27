import 'dart:convert';

import 'package:apptmns/models/verifyotp_model.dart';
import 'package:apptmns/views/auth/dialogs/info_dialogs.dart';
import 'package:apptmns/views/auth/dialogs/no_verified_dialogs.dart';
import 'package:apptmns/views/auth/dialogs/success_dialogs.dart';
import 'package:apptmns/views/auth/login_page.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:loading_animation_widget/loading_animation_widget.dart';

import '../../logic_constants/api_endpoints.dart';
import '../../core/components/network_image.dart';
import '../../core/constants/app_colors.dart';
import '../../core/constants/app_defaults.dart';
import '../../core/constants/app_images.dart';
import '../../core/themes/app_themes.dart';

class NumberVerificationPage extends StatefulWidget {
  const NumberVerificationPage({super.key, required this.userEmail});
  final String userEmail;
  @override
  State<NumberVerificationPage> createState() => _NumberVerificationPageState();
}

class _NumberVerificationPageState extends State<NumberVerificationPage> {
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  String otp = '';
  bool _isLoading = false;
  bool _isResendOTP = false;
  void updateOtp(String value){
    setState(() {
      otp = value;
    });
  }

  Future<void> _resendOtp() async {
    setState(() {
      _isResendOTP = true;
    });
    final Map<String, dynamic> resendOtpModel = {
      'email': widget.userEmail,
    };
    final http = ApiEndpoints.makeNoSSLSecureRequest();
    var result = await http.post(
      Uri.parse(ApiEndpoints.resendRegisterOtp),
      headers: {
        'Content-Type': 'application/json; charset=UTF-8',
      },
      body: jsonEncode(resendOtpModel),
    );
    setState(() {
      _isResendOTP = false;
    });
    if(result.statusCode == 200){
      if (!mounted) return;
      final jsonData = await json.decode(result.body);
      void turnBack(){
        Navigator.pop(context);
      }
      if (!mounted) return;
      showGeneralDialog(
        barrierLabel: 'DialogSuccess',
        barrierDismissible: true,
        context: context,
        pageBuilder: (ctx, anim1, anim2) => InfoDialog(onPressed: turnBack, message: jsonData['message'], buttonTitle: 'Ok'),
        transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
          scale: anim1,
          child: child,
        ),
      );
    }else{
      String errorMessage = 'Có lỗi xảy ra khi gửi lại OTP\nnhưng không có mô tả chi tiết.';
      if(result.body.isNotEmpty){
        final jsonData = json.decode(result.body);
        errorMessage = jsonData['message'];
      }
      if (!mounted) return;
      showGeneralDialog(
        barrierLabel: 'DialogError',
        barrierDismissible: true,
        context: context,
        pageBuilder: (ctx, anim1, anim2) => NoVerifiedDialogs(message: errorMessage),
        transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
          scale: anim1,
          child: child,
        ),
      );
    }
  }

  Future<void> _submitOtp() async {
    if (_formKey.currentState!.validate()) {
      setState(() {
        _isLoading = true;
      });
      _formKey.currentState!.save(); // Lưu giá trị otp

      final otpModel = VerifyOtpModel(email: widget.userEmail, otp: otp);
      final http = ApiEndpoints.makeNoSSLSecureRequest();
      var result = await http.post(
        Uri.parse(ApiEndpoints.verifyRegisterOtp),
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: jsonEncode(otpModel.toJson()),
      );
      setState(() {
        _isLoading = false;
      });
      if(result.statusCode == 200){
        if (!mounted) return;
        final jsonData = await json.decode(result.body);
        void toLoginPage(){
          Navigator.pushAndRemoveUntil(
            context,
            MaterialPageRoute(builder: (context) => const LoginPage()),
                (Route<dynamic> route) => false,
          );
        }
        if (!mounted) return;
        showGeneralDialog(
          barrierLabel: 'DialogSuccess',
          barrierDismissible: true,
          context: context,
          pageBuilder: (ctx, anim1, anim2) => SuccessDialog(onPressed: toLoginPage, message: jsonData['message'], buttonTitle: 'Đăng nhập'),
          transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
            scale: anim1,
            child: child,
          ),
        );
      }else{
        String errorMessage = 'Có lỗi xảy ra\nnhưng không có mô tả chi tiết.';
        if(result.body.isNotEmpty){
          final jsonData = json.decode(result.body);
          errorMessage = jsonData['message'];
        }
        if (!mounted) return;
        showGeneralDialog(
          barrierLabel: 'DialogError',
          barrierDismissible: true,
          context: context,
          pageBuilder: (ctx, anim1, anim2) => NoVerifiedDialogs(message: errorMessage),
          transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
            scale: anim1,
            child: child,
          ),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.scaffoldWithBoxBackground,
      body: SafeArea(
        child: Center(
          child: SingleChildScrollView(
            child: Column(
              children: [
                Container(
                  padding: const EdgeInsets.all(AppDefaults.padding),
                  margin: const EdgeInsets.all(AppDefaults.margin),
                  decoration: BoxDecoration(
                    color: AppColors.scaffoldBackground,
                    borderRadius: AppDefaults.borderRadius,
                  ),
                  child:  Column(
                    children: [
                      const NumberVerificationHeader(),
                      OTPTextFields(onGetOtp: updateOtp,formKey: _formKey),
                      const SizedBox(height: AppDefaults.padding * 3),
                      _isResendOTP ? LoadingAnimationWidget.staggeredDotsWave(color: AppColors.primary, size: 80) :
                      ResendButton(onPressed: _resendOtp,),
                      const SizedBox(height: AppDefaults.padding),
                      _isLoading ? Center(child: LoadingAnimationWidget.staggeredDotsWave(color: AppColors.primary, size: 150)) :
                      VerifyButton(onPressed: _submitOtp,),
                      const SizedBox(height: AppDefaults.padding),
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

class VerifyButton extends StatelessWidget {
  const VerifyButton({
    super.key,
    required this.onPressed,
  });
  final Future<void> Function() onPressed;

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: double.infinity,
      child: ElevatedButton(
        onPressed: onPressed,
        child: const Text('Xác nhận'),
      ),
    );
  }
}

class ResendButton extends StatelessWidget {
  const ResendButton({
    super.key,
    required this.onPressed
  });

  final Future<void> Function() onPressed;

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        const Text('Bạn chưa nhận được mã?'),
        TextButton(
          onPressed: onPressed,
          child: const Text('Gửi lại'),
        ),
      ],
    );
  }
}

class NumberVerificationHeader extends StatelessWidget {
  const NumberVerificationHeader({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        const SizedBox(height: AppDefaults.padding),
        Text(
          'Nhập mã xác nhận',
          style: Theme.of(context).textTheme.titleLarge,
        ),
        const SizedBox(height: AppDefaults.padding),
        SizedBox(
          width: MediaQuery.of(context).size.width * 0.4,
          child: const AspectRatio(
            aspectRatio: 1 / 1,
            child: NetworkImageWithLoader(
              AppImages.numberVerfication,
            ),
          ),
        ),
        const SizedBox(height: AppDefaults.padding * 3),
      ],
    );
  }
}

class OTPTextFields extends StatefulWidget {
  const OTPTextFields({
    super.key,
    required this.formKey,
    required this.onGetOtp
  });

  final GlobalKey<FormState> formKey;
  final void Function(String) onGetOtp;

  @override
  State<OTPTextFields> createState() => _OTPTextFieldsState();
}

class _OTPTextFieldsState extends State<OTPTextFields> {

  final TextEditingController _box1 = TextEditingController();
  final TextEditingController _box2 = TextEditingController();
  final TextEditingController _box3 = TextEditingController();
  final TextEditingController _box4 = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Theme(
      data: AppTheme.defaultTheme.copyWith(
        inputDecorationTheme: AppTheme.otpInputDecorationTheme,
      ),
      child: Form(
        key: widget.formKey,
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceAround,
          children: [
            SizedBox(
              width: 68,
              height: 100,
              child: TextFormField(
                controller: _box1,
                onChanged: (v) {
                  if (v.length == 1) {
                    FocusScope.of(context).nextFocus();
                  } else {
                    FocusScope.of(context).previousFocus();
                  }
                },
                textAlign: TextAlign.center,
                style: Theme.of(context).textTheme.titleLarge?.copyWith(
                      fontWeight: FontWeight.bold,
                    ),
                inputFormatters: [
                  LengthLimitingTextInputFormatter(1),
                  FilteringTextInputFormatter.digitsOnly,
                ],
                keyboardType: TextInputType.number,
                validator: (value) {
                  if(value == null || value.isEmpty) return '';
                  return null;
                },
              ),
            ),
            SizedBox(
              width: 68,
              height: 100,
              child: TextFormField(
                controller: _box2,
                onChanged: (v) {
                  if (v.length == 1) {
                    FocusScope.of(context).nextFocus();
                  } else {
                    FocusScope.of(context).previousFocus();
                  }
                },
                textAlign: TextAlign.center,
                style: Theme.of(context).textTheme.titleLarge?.copyWith(
                      fontWeight: FontWeight.bold,
                    ),
                inputFormatters: [
                  LengthLimitingTextInputFormatter(1),
                  FilteringTextInputFormatter.digitsOnly,
                ],
                keyboardType: TextInputType.number,
                validator: (value) {
                  if(value == null || value.isEmpty) return '';
                  return null;
                },
              ),
            ),
            SizedBox(
              width: 68,
              height: 100,
              child: TextFormField(
                controller: _box3,
                onChanged: (v) {
                  if (v.length == 1) {
                    FocusScope.of(context).nextFocus();
                  } else {
                    FocusScope.of(context).previousFocus();
                  }
                },
                textAlign: TextAlign.center,
                style: Theme.of(context).textTheme.titleLarge?.copyWith(
                      fontWeight: FontWeight.bold,
                    ),
                inputFormatters: [
                  LengthLimitingTextInputFormatter(1),
                  FilteringTextInputFormatter.digitsOnly,
                ],
                keyboardType: TextInputType.number,
                validator: (value) {
                  if(value == null || value.isEmpty) return '';
                  return null;
                },
              ),
            ),
            SizedBox(
              width: 68,
              height: 100,
              child: TextFormField(
                controller: _box4,
                onChanged: (v) {
                  if (v.length == 1) {
                    FocusScope.of(context).nextFocus();
                  } else {
                    FocusScope.of(context).previousFocus();
                  }
                },
                textAlign: TextAlign.center,
                style: Theme.of(context).textTheme.titleLarge?.copyWith(
                      fontWeight: FontWeight.bold,
                    ),
                inputFormatters: [
                  LengthLimitingTextInputFormatter(1),
                  FilteringTextInputFormatter.digitsOnly,
                ],
                keyboardType: TextInputType.number,
                validator: (value) {
                  if(value == null || value.isEmpty) return '';
                  return null;
                },
                onSaved: (value){
                  final String otp = _box1.text+_box2.text+_box3.text+value!;
                  widget.onGetOtp(otp);
                },
              ),
            ),
          ],
        ),
      ),
    );
  }
}
