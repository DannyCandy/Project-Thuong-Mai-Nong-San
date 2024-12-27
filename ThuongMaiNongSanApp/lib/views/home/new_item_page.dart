import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../controllers/product_controller.dart';
import '../../core/components/app_back_button.dart';
import '../../core/components/product_tile_square.dart';
import '../../core/constants/constants.dart';

class NewItemsPage extends StatefulWidget {
  const NewItemsPage({super.key});

  @override
  State<NewItemsPage> createState() => _NewItemsPageState();
}

class _NewItemsPageState extends State<NewItemsPage> {

  late final ProductController productController;
  bool _isLoadingMore = false;
  bool _canLoadMore = true;
  final scrollController = ScrollController();

  void _scrollListener() async {
    if(_isLoadingMore || !_canLoadMore) return;
    if(scrollController.position.pixels == scrollController.position.maxScrollExtent){
      setState(() {
        _isLoadingMore = true;
      });
      productController.currentPage.value = productController.currentPage.value + 1;
      bool canLoadMore = await productController.isGetMoreListProductsPaginated(pageSize: 6,pageIndex: productController.currentPage.value);
      if(!canLoadMore){
        productController.currentPage.value = 1;
      }
      setState(() {
        _isLoadingMore = false;
        _canLoadMore = canLoadMore;
      });
    }
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    productController = Get.find<ProductController>();
    productController.isGetMoreListProductsPaginated(pageSize: 6, pageIndex: 1);
    scrollController.addListener(_scrollListener);
  }

  @override
  void dispose() {
    // TODO: implement dispose
    scrollController.dispose();
    productController.clearListProductsTemp();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Tất cả sản phẩm'),
        leading: const AppBackButton(),
      ),
      body: /*SafeArea(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: AppDefaults.padding),
          child: GridView.builder(
            padding: const EdgeInsets.only(top: AppDefaults.padding),
            gridDelegate: const SliverGridDelegateWithMaxCrossAxisExtent(
              maxCrossAxisExtent: 200,
              childAspectRatio: 0.64,
              mainAxisSpacing: 16,
            ),
            itemCount: 8,
            itemBuilder: (context, index) {
              return ProductTileSquare(
                data: productController.listProductModels[index],
                categoryName: productController.listProductModels[index].categoryName ?? 'Unknown category',
              );
            },
          ),
        ),
      ),*/
      Obx(() {
        if (productController.listProductModels.isEmpty) {
          return const Center(child: CircularProgressIndicator()); // Hoặc một widget khác nếu danh sách trống
        }
        return Stack(
            children: [
              GridView.builder(
                controller: scrollController,
                padding: const EdgeInsets.only(top: AppDefaults.padding),
                gridDelegate: const SliverGridDelegateWithMaxCrossAxisExtent(
                  maxCrossAxisExtent: 200,
                  childAspectRatio: 0.64,
                  mainAxisSpacing: 16,
                ),
                itemCount: productController.listProductModels.length,
                itemBuilder: (context, index) {
                  return ProductTileSquare(
                    data: productController.listProductModels[index],
                    categoryName: '',
                  );
                },
              ),
              if (_isLoadingMore)
                const Positioned(
                  bottom: 16,
                  left: 0,
                  right: 0,
                  child: Center(child: CircularProgressIndicator()),
                ),
            ]
        );
      })
    );
  }
}
