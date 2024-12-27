import 'package:apptmns/core/utils/validators.dart';
import 'package:apptmns/services/user_info_store.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:loading_animation_widget/loading_animation_widget.dart';
import 'package:uuid/uuid.dart';
import '../../../controllers/address_controller.dart';
import '../../../core/components/app_radio.dart';
import '../../../core/constants/app_colors.dart';
import '../../../core/constants/app_defaults.dart';

import '../../../core/components/app_back_button.dart';

class NewAddressPage extends StatefulWidget {
  const NewAddressPage({super.key});

  @override
  State<NewAddressPage> createState() => _NewAddressPageState();
}

class _NewAddressPageState extends State<NewAddressPage> {

  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  final TextEditingController _addressController = TextEditingController();
  final TextEditingController _phoneController = TextEditingController();
  final AddressController addressController = Get.put(AddressController());
  bool _isDefaultShippingAddress = false;

  bool checkFormIsOk(){
    return _formKey.currentState?.validate() ?? false;
  }

  @override
  void dispose() {
    // TODO: implement dispose
    _addressController.dispose();
    _phoneController.dispose();
    super.dispose();
  }

  Future<void> onSubmitAddNewAddress(bool isFormOkay) async {
    if (isFormOkay) {
      var uuid = const Uuid();
      final Map<String, dynamic> addNewAddressData = {
        'userId': UserPreferences.getUserId()!,
        'idttlh': uuid.v4(),
        'dcgiaoHang':_addressController.text,
        'phone':_phoneController.text,
        'defaultPos':_isDefaultShippingAddress,
      };
      await addressController.onCreateAddress(addNewAddressData);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.cardColor,
      appBar: AppBar(
        leading: const AppBackButton(),
        title: const Text(
          'Địa chỉ mới',
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
          child: Form(
            key: _formKey,
            child: Column(
              mainAxisSize: MainAxisSize.min,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                /* <----  Full Name -----> */
                const Text("Địa chỉ"),
                const SizedBox(height: 8),
                TextFormField(
                  controller: _addressController,
                  keyboardType: TextInputType.text,
                  textInputAction: TextInputAction.next,
                  validator: Validators.requiredWithFieldName('Địa chỉ').call,
                ),
                const SizedBox(height: AppDefaults.padding),
            
                /* <---- Phone Number -----> */
                const Text("Số điện thoại"),
                const SizedBox(height: 8),
                TextFormField(
                  controller: _phoneController,
                  keyboardType: TextInputType.number,
                  textInputAction: TextInputAction.next,
                  validator: Validators.phone.call,
                ),
                const SizedBox(height: AppDefaults.padding),
            
                FormField<bool>(
                  initialValue: _isDefaultShippingAddress,
                  builder: (FormFieldState<bool> state) {
                    return Padding(
                      padding: const EdgeInsets.symmetric(vertical: AppDefaults.padding),
                      child: Row(
                        children: [
                          GestureDetector(
                            onTap: () {
                              // Cập nhật trạng thái khi RadioButton được nhấn
                              setState(() {
                                _isDefaultShippingAddress = !_isDefaultShippingAddress;
                                state.didChange(_isDefaultShippingAddress); // Thông báo thay đổi cho FormField
                              });
                            },
                            child: Row(
                                children: [
                                  AppRadio(isActive: _isDefaultShippingAddress),
                                  const SizedBox(width: AppDefaults.padding),
                                  const Text('Đặt địa chỉ mặc định'),
                                ]
                            ),
                          ),
                        ],
                      ),
                    );
                  }
                ),
                const SizedBox(height: AppDefaults.padding),
                SizedBox(
                  width: double.infinity,
                  child: Obx(() =>
                  addressController.isLoading.value ?
                    Center(child: LoadingAnimationWidget.staggeredDotsWave(color: AppColors.primary, size: 150))
                        :
                    ElevatedButton(
                      child: const Text('Lưu địa chỉ'),
                      onPressed: () async {
                        bool isOk = checkFormIsOk();
                        if(isOk){
                          await onSubmitAddNewAddress(isOk);
                        }
                      },
                    ),)
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
