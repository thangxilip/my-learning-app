<script setup lang="ts">
import { Plus } from 'lucide-vue-next'

import { Card } from '@/components/ui/card'

defineProps<{
  disabled?: boolean
}>()

const emit = defineEmits<{
  (e: 'click'): void
}>()
</script>

<template>
  <Card
    role="button"
    tabindex="0"
    :class="[
      'h-full min-h-[320px] cursor-pointer items-center justify-center gap-3 border-dashed py-8 shadow-none transition-colors',
      'hover:border-primary/40 hover:bg-muted/30',
      disabled && 'pointer-events-none opacity-60',
    ]"
    @click="emit('click')"
    @keydown.enter.prevent="emit('click')"
    @keydown.space.prevent="emit('click')"
  >
    <div
      class="flex size-12 items-center justify-center rounded-full border border-dashed border-muted-foreground/50"
    >
      <Plus class="size-5 text-muted-foreground" />
    </div>
    <p class="text-sm font-medium text-muted-foreground">Add deck</p>
  </Card>
</template>
