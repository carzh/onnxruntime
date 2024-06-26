// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// This file is generated by compile_sparse_attention.py using triton AoT compiler

#pragma once
#include "contrib_ops/cuda/sparse/sparse_attention_v1/sparse_attention_common.h"

namespace onnxruntime {
namespace contrib {
namespace cuda {
namespace sparse_attention_v1 {

// launcher for: sparse_attention_fp16_sm90_64x0x64x0x64x2_warps4xstages3
Status sparse_attention_fp16_sm90_ec2c5ffe(SparseAttentionParams& params);

Status sparse_attention_fp16_sm90_64x0x64x0x64x2_warps4xstages3(SparseAttentionParams& params) {
  return sparse_attention_fp16_sm90_ec2c5ffe(params);
}

// load for: sparse_attention_fp16_sm90_64x0x64x0x64x2_warps4xstages3
void load_sparse_attention_fp16_sm90_ec2c5ffe();
void load_sparse_attention_fp16_sm90_64x0x64x0x64x2_warps4xstages3() {
  load_sparse_attention_fp16_sm90_ec2c5ffe();
}

// unload for: sparse_attention_fp16_sm90_64x0x64x0x64x2_warps4xstages3
void unload_sparse_attention_fp16_sm90_ec2c5ffe();
void unload_sparse_attention_fp16_sm90_64x0x64x0x64x2_warps4xstages3() {
  unload_sparse_attention_fp16_sm90_ec2c5ffe();
}

// launcher for: sparse_attention_fp16_sm90_64x1x64x1x64x2_warps4xstages3
Status sparse_attention_fp16_sm90_f6d43951(SparseAttentionParams& params);

Status sparse_attention_fp16_sm90_64x1x64x1x64x2_warps4xstages3(SparseAttentionParams& params) {
  return sparse_attention_fp16_sm90_f6d43951(params);
}

// load for: sparse_attention_fp16_sm90_64x1x64x1x64x2_warps4xstages3
void load_sparse_attention_fp16_sm90_f6d43951();
void load_sparse_attention_fp16_sm90_64x1x64x1x64x2_warps4xstages3() {
  load_sparse_attention_fp16_sm90_f6d43951();
}

// unload for: sparse_attention_fp16_sm90_64x1x64x1x64x2_warps4xstages3
void unload_sparse_attention_fp16_sm90_f6d43951();
void unload_sparse_attention_fp16_sm90_64x1x64x1x64x2_warps4xstages3() {
  unload_sparse_attention_fp16_sm90_f6d43951();
}

typedef Status (*kernel_func_t)(SparseAttentionParams& params);
kernel_func_t sparse_attention_fp16_sm90_kernels[] = {
    sparse_attention_fp16_sm90_64x0x64x0x64x2_warps4xstages3,
    sparse_attention_fp16_sm90_64x1x64x1x64x2_warps4xstages3,
};

int sparse_attention_fp16_sm90_get_num_algos(void) {
  return (int)sizeof(sparse_attention_fp16_sm90_kernels);
}

Status sparse_attention_fp16_sm90(SparseAttentionParams& params, int algo_id) {
  assert(algo_id < (int)sizeof(sparse_attention_fp16_sm90_kernels));
  return sparse_attention_fp16_sm90_kernels[algo_id](params);
}

void load_sparse_attention_fp16_sm90(void) {
  load_sparse_attention_fp16_sm90_64x0x64x0x64x2_warps4xstages3();
  load_sparse_attention_fp16_sm90_64x1x64x1x64x2_warps4xstages3();
}

void unload_sparse_attention_fp16_sm90(void) {
  unload_sparse_attention_fp16_sm90_64x0x64x0x64x2_warps4xstages3();
  unload_sparse_attention_fp16_sm90_64x1x64x1x64x2_warps4xstages3();
}

Status sparse_attention_fp16_sm90_default(SparseAttentionParams& params) {
  return sparse_attention_fp16_sm90(params, 0);
}

}  // namespace sparse_attention_v1
}  // namespace cuda
}  // namespace contrib
}  // namespace onnxruntime
