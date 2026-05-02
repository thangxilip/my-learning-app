<script setup lang="ts">
import { computed } from 'vue'

import { Button } from '@/components/ui/button'
import { useAuthStore } from '@/stores/auth'
import { useRouter } from 'vue-router'

const auth = useAuthStore()
const router = useRouter()

const greeting = computed(() => 'Welcome back')

function signOut() {
  auth.clearSession()
  void router.push({ name: 'login' })
}
</script>

<template>
  <main class="min-h-screen bg-background px-6 py-12 text-foreground">
    <div class="mx-auto flex max-w-lg flex-col gap-6">
      <div class="space-y-2">
        <p class="text-sm font-medium text-muted-foreground">Lingua Learn</p>
        <h1 class="text-3xl font-semibold tracking-tight">{{ greeting }}</h1>
        <p class="text-muted-foreground">
          You are signed in. Your dashboard will live here.
        </p>
      </div>
      <div>
        <Button variant="outline" type="button" @click="signOut">Sign out</Button>
      </div>
    </div>
  </main>
</template>
