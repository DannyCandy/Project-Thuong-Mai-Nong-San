

import 'package:apptmns/models/product_model.dart';
import 'package:apptmns/views/auth/dialogs/no_verified_dialogs.dart';
import 'package:apptmns/views/auth/dialogs/success_dialogs.dart';
import 'package:get/get.dart';

class SaveController extends GetxController {
  RxList<Product> listSavedItem = <Product>[].obs;
  RxList<Product> listProductsItemAtFirst = <Product>[].obs;

  void assignListProductsItem(RxList<Product> listItems) {
    listProductsItemAtFirst.assignAll(listItems);
  }

  void addToFavourite(String productId){
    bool isExist = listSavedItem.any((item) => item.idsp == productId);
    if(isExist){
      Get.dialog(const NoVerifiedDialogs(message: "Sản phẩm đã tồn tại trong danh sách yêu thích.",));
      return;
    }else{
      Product item = listProductsItemAtFirst.firstWhere((i) => i.idsp == productId);
      listSavedItem.add(item);
    }
    Get.dialog(SuccessDialog(
        onPressed: () => Get.back(),
        message: "Thêm sản phẩm vào danh sách yêu thích thành công.",
        buttonTitle: "Ok"));
    return;
  }

  void removeFromFavourite(String productIdToDel){
    listSavedItem.removeWhere((item) => item.idsp == productIdToDel);
  }

  void clearFavourite(){
    listSavedItem.clear();
  }
}