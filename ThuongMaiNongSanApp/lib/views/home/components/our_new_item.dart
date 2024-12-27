import 'package:apptmns/controllers/product_controller.dart';
import 'package:apptmns/controllers/save_controller.dart';
import 'package:apptmns/views/home/new_item_page.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../core/components/product_tile_square.dart';
import '../../../core/components/title_and_action_button.dart';
import '../../../core/constants/constants.dart';
import '../../../core/routes/app_routes.dart';

class OurNewItem extends StatelessWidget {
  const OurNewItem({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    Get.lazyPut(()=>ProductController());
    final productController = Get.find<ProductController>();
    Get.lazyPut(()=>SaveController());
    WidgetsBinding.instance.addPostFrameCallback((_) {
      productController.getListProductsForFirst();
    });

    return Column(
      children: [
        TitleAndActionButton(
          title: 'Sản phẩm bán chạy',
          onTap: () {
            Get.toNamed(AppRoutes.newItems);
          },
        ),
        SingleChildScrollView(
          padding: const EdgeInsets.only(left: AppDefaults.padding),
          scrollDirection: Axis.horizontal,
          child: Obx(() =>
              Row(
                children: List.generate(
                  productController.listAllProductForEntrance.length > 3 ? 4 : productController.listAllProductForEntrance.length,
                      (index) => ProductTileSquare(
                    data: productController.listAllProductForEntrance[index],
                    categoryName: productController.listAllProductForEntrance[index].categoryName ?? 'Không có loại',
                  ),
                ),
              ),)
        ),
      ],
    );
  }
}
