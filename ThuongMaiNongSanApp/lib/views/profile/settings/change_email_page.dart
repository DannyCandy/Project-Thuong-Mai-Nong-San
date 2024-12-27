
import 'package:apptmns/services/user_info_store.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:loading_animation_widget/loading_animation_widget.dart';

import '../../../controllers/user_controller.dart';
import '../../../core/components/app_back_button.dart';
import '../../../core/constants/constants.dart';
import '../../../core/utils/validators.dart';

class ChangeEmailPage extends StatefulWidget {
  const ChangeEmailPage({super.key});

  @override
  State<ChangeEmailPage> createState() => _ChangeEmailPageState();
}

class _ChangeEmailPageState extends State<ChangeEmailPage> {
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  final TextEditingController _emailController = TextEditingController();

  final String userID = UserPreferences.getUserId()!;
  final UserController _profileController = Get.put(UserController());
  /*bool _isLoading = false;*/

  @override
  void dispose() {
    _emailController.dispose();
    super.dispose();
  }

  bool checkFormIsOk(){
    return _formKey.currentState?.validate() ?? false;
  }

  Future<void> onSubmitChangeEmail(bool isFormOkay) async {
    if (isFormOkay) {
      final Map<String, String> changeEmailData = {
        'userId':userID,
        'email':_emailController.text
      };
      /*final http = ApiEndpoints.makeNoSSLSecureRequest();
      var result = await http.post(
        Uri.parse(ApiEndpoints.changeUserEmail),
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          'Authorization': 'Bearer ${UserPreferences.getUserToken()}',
        },
        body: jsonEncode(changeEmailData),
      );*/

      /*if (result.statusCode == 200) {
        await UserPreferences.setUserEmail(_emailController.text);
        return true;
      }*/
      await _profileController.updateUserEmail(changeEmailData);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: const AppBackButton(),
        title: const Text(
          'Đổi email',
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
                children: [/*
                  const Text("Current email"),
                  const SizedBox(height: 8),
                  Obx(() => TextFormField(
                    initialValue: _profileController.listUserInfo['email'],
                    readOnly: true,
                  ),),

                  const SizedBox(height: AppDefaults.padding),*/
                  const Text("Email mới"),
                  const SizedBox(height: 8),
                  TextFormField(
                    controller: _emailController,
                    keyboardType: TextInputType.emailAddress,
                    validator: Validators.registerEmail.call,
                    textInputAction: TextInputAction.next,
                  ),
                  const SizedBox(height: AppDefaults.padding),

                  SizedBox(
                    width: double.infinity,
                    child: Obx(() =>
                    _profileController.isLoading.value ? Center(child: LoadingAnimationWidget.staggeredDotsWave(color: AppColors.primary, size: 150))
                        :
                    ElevatedButton(
                      child: const Text('Cập nhật email'),
                      onPressed: () async {
                        bool isOk = checkFormIsOk();
                        if(isOk){
                          await onSubmitChangeEmail(isOk);
                        }
                        _emailController.clear();
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
