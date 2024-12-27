import 'package:apptmns/controllers/address_controller.dart';
import 'package:apptmns/views/profile/address/update_address_page.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';
import 'package:get/get.dart';
import 'package:loading_animation_widget/loading_animation_widget.dart';

import '../../../core/components/app_back_button.dart';
import '../../../core/components/app_radio.dart';
import '../../../core/constants/constants.dart';
import '../../../core/routes/app_routes.dart';

class AddressPage extends StatelessWidget {
  const AddressPage({super.key});
/*
  @override
  State<AddressPage> createState() => _AddressPageState();
}

class _AddressPageState extends State<AddressPage> {

  List<Map<String,dynamic>> listAddress = [];
  bool _isLoading = false;

  Future<void> fetchAddresses() async {
    setState(() async {
      _isLoading = true;
    });
    final http = ApiEndpoints.makeNoSSLSecureRequest();
    final response = await http.get(
      Uri.parse("${ApiEndpoints.getAllUserAddress}?userId=${UserPreferences.getUserId()}"),
      headers: {
        'Content-Type': 'application/json; charset=UTF-8',
        'Authorization': 'Bearer ${UserPreferences.getUserToken()}',
      }
    );
    if (response.statusCode == 200) {
      setState(() async {
        listAddress = await json.decode(response.body);
        _isLoading = false;
      });
    } else {
      if(!mounted) return;
      showGeneralDialog(
        barrierLabel: 'DialogError',
        barrierDismissible: true,
        context: context,
        pageBuilder: (ctx, anim1, anim2) => const NoVerifiedDialogs(message: 'Xảy ra lỗi khi tải dữ liệu người dùng.'),
        transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
          scale: anim1,
          child: child,
        ),
      );
    }
  }

  Future<void> onDeleteAddress(String idttlh) async {
    setState(() async {
      _isLoading = true;
    });
    final http = ApiEndpoints.makeNoSSLSecureRequest();
    final response = await http.put(
        Uri.parse("${ApiEndpoints.deleteUserAddress}?ttlhId=$idttlh"),
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          'Authorization': 'Bearer ${UserPreferences.getUserToken()}',
        }
    );

    if(response.body.isNotEmpty){
      var resData = await jsonDecode(response.body);
      if(response.statusCode == 200){
        await fetchAddresses();
        if(!mounted) return;
        showGeneralDialog(
          barrierLabel: 'DialogSuccess',
          barrierDismissible: true,
          context: context,
          pageBuilder: (ctx, anim1, anim2) => SuccessDialog(message: resData['message']
              ,buttonTitle: 'Ok',
              onPressed: () => Navigator.pop(context)),
          transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
            scale: anim1,
            child: child,
          ),
        );
      }else{
        if(!mounted) return;
        showGeneralDialog(
          barrierLabel: 'ErrorWithMess',
          barrierDismissible: true,
          context: context,
          pageBuilder: (ctx, anim1, anim2) => NoVerifiedDialogs(message: resData['message']),
          transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
            scale: anim1,
            child: child,
          ),
        );
      }
    }else{
      if(!mounted) return;
      showGeneralDialog(
        barrierLabel: 'Error',
        barrierDismissible: true,
        context: context,
        pageBuilder: (ctx, anim1, anim2) => const NoVerifiedDialogs(message: 'Xảy ra lỗi khi xóa địa chỉ!'),
        transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
          scale: anim1,
          child: child,
        ),
      );
    }
    setState(() async {
      _isLoading = false;
    });
    return;
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    fetchAddresses();
  }

  @override
  void dispose() {
    // TODO: implement dispose
    super.dispose();
  }*/

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => AddressController());
    final addressController = Get.find<AddressController>();

    return Scaffold(
      backgroundColor: AppColors.cardColor,
      appBar: AppBar(
        leading: const AppBackButton(),
        title: const Text(
          'Địa chỉ giao hàng',
        ),
      ),
      body:
      Obx(() =>
        addressController.isLoading.value ? Center(child: LoadingAnimationWidget.staggeredDotsWave(color: AppColors.primary, size: 200))
        :
        Container(
          margin: const EdgeInsets.all(AppDefaults.margin),
          padding: const EdgeInsets.all(AppDefaults.padding),
          decoration: BoxDecoration(
            color: AppColors.scaffoldBackground,
            borderRadius: AppDefaults.borderRadius,
          ),
          child: Stack(
            children: [
              Obx(() => ListView.separated(
                itemBuilder: (context, index) {
                  Map<String, dynamic> address = addressController.listUserAddress[index];
                  return AddressTile(
                    idAddress: address['idttlh'],
                    label: address['dcgiaoHang'],
                    number: address['phone'],
                    isActive: address['defaultPos'] as bool,
                  );
                },
                itemCount: addressController.listUserAddress.length,
                separatorBuilder: (context, index) =>
                const Divider(thickness: 0.2),)
              ),
              Positioned(
                bottom: 16,
                right: 16,
                child: FloatingActionButton(
                  onPressed: () {
                    Navigator.pushNamed(context, AppRoutes.newAddress);
                  },
                  backgroundColor: AppColors.primary,
                  splashColor: AppColors.primary,
                  child: const Icon(Icons.add),
                ),
              ),
            ],
          ),
        ),)
    );
  }
}

class AddressTile extends StatelessWidget {
  const AddressTile({
    super.key,
    required this.idAddress,
    required this.label,
    required this.number,
    required this.isActive,
  });

  final String idAddress;
  final String label;
  final String number;
  final bool isActive;

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(() => AddressController());
    final addressController = Get.find<AddressController>();

    return Obx(() => addressController.isLoading.value ?
    const Center(child: CircularProgressIndicator(),)
        :
    Padding(
      padding: const EdgeInsets.all(8.0),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          AppRadio(isActive: isActive),
          const SizedBox(width: AppDefaults.padding),
          Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              const SizedBox(height: 4),
              Text(isActive ? 'Mặc định' : ''),
              Text(
                label,
                style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                  color: Colors.black,
                ),
              ),
              const SizedBox(height: 4),
              Text(
                number,
                style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                  color: Colors.black,
                ),
              )
            ],
          ),
          const Spacer(),
          Column(
            children: [
              IconButton(
                onPressed: () {
                  Get.to(() => UpdateAddressPage(idTtlh: idAddress,));
                },
                icon: SvgPicture.asset(AppIcons.edit),
                constraints: const BoxConstraints(),
                iconSize: 14,
              ),
              const SizedBox(height: AppDefaults.margin / 2),
              IconButton(
                onPressed: () async {
                  await addressController.onDeleteAddress(idAddress);
                },
                icon: SvgPicture.asset(AppIcons.deleteOutline),
                constraints: const BoxConstraints(),
                iconSize: 14,
              ),
            ],
          )
        ],
      ),
    ));
  }
}
