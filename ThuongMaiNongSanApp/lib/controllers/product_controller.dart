
import 'package:apptmns/controllers/save_controller.dart';
import 'package:dio/dio.dart';
import 'package:flutter_credit_card/extension.dart';
import 'package:get/get.dart';

import '../logic_constants/api_endpoints.dart';
import '../models/product_model.dart';
import '../models/product_item_model.dart';
import '../services/dio_db_connect.dart';
import '../views/auth/dialogs/no_verified_dialogs.dart';

class ProductController extends GetxController {
  List<Map<String, dynamic>> listProductsTemp = <Map<String, dynamic>>[];
  RxList<Map<String, dynamic>> listCategories = <Map<String, dynamic>>[].obs;
  RxList<Product> listProductModels = <Product>[].obs;
  RxList<Product> listAllProductForEntrance = <Product>[].obs;
  Rx<ProductItemModel> singleProductItem = ProductItemModel().obs;

  Rx<String> searchName = ''.obs;
  RxList<String> listSearchName = <String>[].obs;

  RxInt currentPage = 1.obs;

  void setSearchName(String searchWord){
    searchName.value = searchWord;
  }

  //Icon category
  final List<String> linkImages = [
    'https://i.imgur.com/mGRqfnc.png',//Xe đẩy em bé
    'https://i.imgur.com/fwyz4oC.png',//Hộp bút
    'https://i.imgur.com/O2ZX5nR.png',//Ví nhỏ
    'https://i.imgur.com/wJBopjL.png',//Tay nâng cây
    'https://i.imgur.com/sxGf76e.png',//Kính
    'https://i.imgur.com/BPvKeXl.png',//Túi xách
  ];
  int _currentIndex = 0;

  String getNextItem() {
    if (linkImages.isEmpty) {
      return ''; // Trả về một giá trị mặc định nếu danh sách rỗng
    }

    // Lấy phần tử hiện tại
    String currentItem = linkImages[_currentIndex];

    // Cập nhật chỉ số để lấy phần tử tiếp theo (nếu hết thì quay lại đầu)
    _currentIndex = (_currentIndex + 1) % linkImages.length;

    return currentItem;
  }

  String getProductImageById(String productId){
    Product product = listAllProductForEntrance.firstWhere((i) => i.idsp == productId);
    return '${ApiEndpoints.apiUri}${product.hinhAnhSp}';
  }

  String getProductNameById(String productId){
    Product product = listAllProductForEntrance.firstWhere((i) => i.idsp == productId);
    return '${product.spName}';
  }

  Future<void> getSingleProducts(String productId) async {
    try{
      if(productId.isNullOrEmpty){
        Get.dialog(
          const NoVerifiedDialogs(
            message: 'Yêu cầu không hợp lệ, không tìm được sản phẩm',
          ),
        );
        return;
      }
      final DioClient dioClient = DioClient();
      var result = await dioClient.dio.get(ApiEndpoints.getProductById,
        options: Options(
            headers: {
              'Content-Type': 'application/json; charset=UTF-8',
            }
        ),
        queryParameters: {
          'id':productId
        },
      );
      if(result.statusCode == 200){
        singleProductItem.value = ProductItemModel.fromJson(result.data);
      }
    }on DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
        NoVerifiedDialogs(message: errMess)
      );
    }catch(e){
      return;
    }
  }

  Future<void> getListProductsForFirst() async {
    /*final http = ApiEndpoints.makeNoSSLSecureRequest();
    var result = await http.post(
      Uri.parse(ApiEndpoints.getAllProducts),
      headers: {
        'Content-Type': 'application/json; charset=UTF-8',
      },
    );*/
    try{
      final DioClient dioClient = DioClient();
      var result = await dioClient.dio.get(ApiEndpoints.getAllProducts,
        options: Options(
            headers: {
              'Content-Type': 'application/json; charset=UTF-8',
            }
        ),
      );
      if(result.statusCode == 200){
        /*final jsonProductsData = await json.decode(result.body);*/
        List<Map<String, dynamic>> listAllProductsTemp = (result.data as List).cast<Map<String, dynamic>>();
        listAllProductForEntrance.assignAll(
          listAllProductsTemp.map((json) => Product.fromJson(json)).toList(),
        );

        Get.find<SaveController>().assignListProductsItem(listAllProductForEntrance);
      }
    }on DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
          NoVerifiedDialogs(message: errMess)
      );
    }catch(e){
      return;
    }
  }

  Future<void> getListCategories() async {
    /*final http = ApiEndpoints.makeNoSSLSecureRequest();
    var result = await http.post(
      Uri.parse(ApiEndpoints.getAllCategories),
      headers: {
        'Content-Type': 'application/json; charset=UTF-8',
      },
    );*/
    try{
      final DioClient dioClient = DioClient();
      var result = await dioClient.dio.get(ApiEndpoints.getAllCategories,
        options: Options(
            headers: {
              'Content-Type': 'application/json; charset=UTF-8',
            }
        ),
      );
      if(result.statusCode == 200){
        final jsonProductsData = result.data;
        listCategories.assignAll(jsonProductsData.cast<Map<String, dynamic>>());
      }
    }on DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
          NoVerifiedDialogs(message: errMess)
      );
    }catch(e){
      return;
    }
  }

  Future<bool> isGetMoreListProductsPaginated({required int pageSize, required int pageIndex, String category = "",
    String orderBy = "",
    String searchName = "",}) async {
    /*final http = ApiEndpoints.makeNoSSLSecureRequest();
    var result = await http.post(
      Uri.parse("${ApiEndpoints.getProductByPaginated}"
          "?pageSize=$pageSize&pageIndex=$pageIndex&category=$category&orderBy=$orderBy&searchName=$searchName"),
      headers: {
        'Content-Type': 'application/json; charset=UTF-8',
      },
    );*/

    try{
      final DioClient dioClient = DioClient();
      var result = await dioClient.dio.get(ApiEndpoints.getProductByPaginated,
          options: Options(
            headers: {
              'Content-Type': 'application/json; charset=UTF-8',
            },
          ),
          queryParameters: {
            'pageSize':pageSize,
            'pageIndex':pageIndex,
            'category':category,
            'orderBy':orderBy,
            'searchName':searchName
          }
      );

      if(result.statusCode == 200){

        final List<Map<String, dynamic>> tempData = (result.data as List).cast<Map<String, dynamic>>();

        if(tempData.isEmpty){
          return false;
        }

        listProductsTemp.addAll(tempData);
        listProductModels.assignAll(
          listProductsTemp.map((json) => Product.fromJson(json)).toList(),
        );
        return true;
      }
      return false;
    }on DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
          NoVerifiedDialogs(message: errMess)
      );
      return false;
    }catch(e){
      return false;
    }
  }

  void clearListProductsTemp(){
    listProductsTemp.clear();
    listProductModels.clear();
  }
}
