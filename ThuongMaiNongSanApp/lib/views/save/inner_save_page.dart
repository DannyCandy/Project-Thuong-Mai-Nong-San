import 'package:apptmns/controllers/save_controller.dart';
import 'package:apptmns/logic_constants/api_endpoints.dart';
import 'package:apptmns/views/home/product_details_page.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class InnerSavePage extends StatelessWidget {
  const InnerSavePage({super.key});

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(()=>SaveController());
    final saveController = Get.find<SaveController>();

    return Scaffold(
      backgroundColor: Colors.white,
      appBar: AppBar(
        centerTitle: false,
        elevation: 0,
        backgroundColor: const Color(0xFF00BF6D),
        foregroundColor: Colors.white,
        title: const Text("Các sản phẩm đã lưu"),
      ),
      body: SafeArea(
        child: Obx(() => ListView(
          children: [
            // For demo
            ...List.generate(
              saveController.listSavedItem.length,
                  (index) => SaveItemCard(
                name: saveController.listSavedItem[index].spName!,
                image: saveController.listSavedItem[index].hinhAnhSp!,
                time: saveController.listSavedItem[index].price.toString(),
                productId: saveController.listSavedItem[index].idsp!,
                press: () {
                  Get.to(() => ProductDetailsPage(productId: saveController.listSavedItem[index].idsp!));
                },
              ),
            ),
          ],
        ),)
      ),
    );
  }
}

class SaveItemCard extends StatelessWidget {
  const SaveItemCard({
    super.key,
    required this.name,
    required this.time,
    required this.image,
    required this.press,
    required this.productId,
  });

  final String name, time, image, productId;
  final VoidCallback press;

  @override
  Widget build(BuildContext context) {
    return ListTile(
      contentPadding: const EdgeInsets.symmetric(
        horizontal: 16.0,
        vertical: 16.0 / 2,
      ),
      onTap: press,
      leading: CircleAvatarWithActiveIndicator(
        image: image,
        radius: 28,
      ),
      title: Text(name),
      subtitle: Padding(
        padding: const EdgeInsets.symmetric(vertical: 16.0 / 2),
        child: Row(
          children: [
            const SizedBox(width: 16.0 / 2),
            Text(
              time,
              style: TextStyle(
                color: Theme.of(context)
                    .textTheme
                    .bodyLarge!
                    .color!
                    .withOpacity(0.64),
              ),
            ),
          ],
        ),
      ),
      trailing: /*Icon(
        Icons.delete,
        color: Theme.of(context).primaryColor,*/
      IconButton(
        onPressed: () {
          Get.find<SaveController>().removeFromFavourite(productId);
        },
        iconSize: 28,
        constraints: const BoxConstraints(minHeight: 28, minWidth: 28),
        icon: Icon(
          Icons.delete,
          color: Theme.of(context).primaryColor,
        ),
      ),
    );
  }
}

class CircleAvatarWithActiveIndicator extends StatelessWidget {
  const CircleAvatarWithActiveIndicator({
    super.key,
    this.image,
    this.radius = 24,
  });

  final String? image;
  final double? radius;

  @override
  Widget build(BuildContext context) {
    return Stack(
      children: [
        CircleAvatar(
          radius: radius,
          backgroundImage: NetworkImage(ApiEndpoints.apiUri+image!),
        ),
      ],
    );
  }
}
