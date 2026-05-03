<script setup lang="ts">
import { ClipboardList } from 'lucide-vue-next'
</script>

<template>
  <div
    class="flex min-h-[40vh] flex-col items-center justify-center gap-3 rounded-xl border border-dashed border-border bg-muted/20 px-6 py-16 text-center"
  >
    <div
      class="flex size-12 items-center justify-center rounded-full bg-muted text-muted-foreground"
    >
      <ClipboardList class="size-6" />
    </div>
    <div class="space-y-1">
      <h2 class="text-lg font-semibold tracking-tight">Board of Todo</h2>
      <p class="max-w-sm text-sm text-muted-foreground">
        This workspace will host your todo board. The flashcards tab is ready;
        boards will ship in a later phase.
      </p>
    </div>
  </div>
</template>
