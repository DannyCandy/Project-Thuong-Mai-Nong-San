import 'package:apptmns/logic_constants/api_endpoints.dart';
import 'package:apptmns/models/product_model.dart';
import 'package:flutter/material.dart';

import '../../views/home/product_details_page.dart';
import '../constants/constants.dart';
import 'network_image.dart';

class ProductTileSquare extends StatelessWidget {
  const ProductTileSquare({
    super.key,
    required this.data,
    required this.categoryName,
  });

  final Product data;
  final String categoryName;

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: AppDefaults.padding / 2),
      child: Material(
        borderRadius: AppDefaults.borderRadius,
        color: AppColors.scaffoldBackground,
        child: InkWell(
          borderRadius: AppDefaults.borderRadius,
          onTap: () => Navigator.push(
            context,
            MaterialPageRoute(
              builder: (context) => ProductDetailsPage(productId: data.idsp!),
            ),
          ),

          child: Container(
            width: 176,
            height: 336,
            padding: const EdgeInsets.all(AppDefaults.padding),
            decoration: BoxDecoration(
              border: Border.all(width: 0.1, color: AppColors.placeholder),
              borderRadius: AppDefaults.borderRadius,
            ),

            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Expanded(
                  child: Padding(
                    padding: const EdgeInsets.all(AppDefaults.padding / 2),
                    child: AspectRatio(
                      aspectRatio: 3 / 2,
                      child: NetworkImageWithLoader(
                        ApiEndpoints.apiUri+data.hinhAnhSp!,
                        fit: BoxFit.fitWidth,
                      ),
                    ),
                  ),
                ),
                const SizedBox(height: 8),
                Text(
                  data.spName!,
                  style: Theme.of(context)
                      .textTheme
                      .titleMedium
                      ?.copyWith(color: Colors.black),
                  maxLines: 2,
                  overflow: TextOverflow.ellipsis,
                ),
                /*const Spacer(),*/
                const SizedBox(height: 4),
                Text(
                  categoryName,
                ),
                const SizedBox(height: 4),
                Row(
                  mainAxisSize: MainAxisSize.min,
                  crossAxisAlignment: CrossAxisAlignment.end,
                  children: [
                    Text(
                      '${data.price!.toInt()} VND',
                      style: Theme.of(context)
                          .textTheme
                          .titleLarge
                          ?.copyWith(color: Colors.black),
                    ),
                    const SizedBox(
                      width: 4,
                    ),
                  ],
                )
              ],
            ),
          ),
        ),
      ),
    );
  }
}
