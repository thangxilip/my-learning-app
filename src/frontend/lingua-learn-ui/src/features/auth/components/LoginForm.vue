<script setup lang="ts">
import { computed, reactive, ref } from 'vue'
import { Eye, EyeOff, LoaderCircle, LockKeyhole, Mail } from 'lucide-vue-next'

import { Button } from '@/components/ui/button'
import { Checkbox } from '@/components/ui/checkbox'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import type { LoginFormValues } from '@/features/auth/types'

const emit = defineEmits<{
  submit: [values: LoginFormValues]
}>()

const form = reactive<LoginFormValues>({
  email: '',
  password: '',
  rememberMe: true,
})

const showPassword = ref(false)
const isSubmitting = ref(false)

const canSubmit = computed(() => form.email.trim().length > 0 && form.password.length > 0)

function handleSubmit() {
  if (!canSubmit.value || isSubmitting.value) return

  isSubmitting.value = true

  window.setTimeout(() => {
    emit('submit', {
      email: form.email.trim(),
      password: form.password,
      rememberMe: form.rememberMe,
    })
    isSubmitting.value = false
  }, 450)
}
</script>

<template>
  <form class="space-y-5" novalidate @submit.prevent="handleSubmit">
    <div class="space-y-2">
      <Label for="email">Email</Label>
      <div class="relative">
        <Mail class="pointer-events-none absolute left-3 top-1/2 size-4 -translate-y-1/2 text-muted-foreground" />
        <Input
          id="email"
          v-model="form.email"
          autocomplete="email"
          class="h-10 pl-9"
          inputmode="email"
          name="email"
          placeholder="you@example.com"
          type="email"
        />
      </div>
    </div>

    <div class="space-y-2">
      <div class="flex items-center justify-between gap-3">
        <Label for="password">Password</Label>
        <a class="text-sm font-medium text-foreground underline-offset-4 hover:underline" href="/forgot-password">
          Forgot password?
        </a>
      </div>
      <div class="relative">
        <LockKeyhole class="pointer-events-none absolute left-3 top-1/2 size-4 -translate-y-1/2 text-muted-foreground" />
        <Input
          id="password"
          v-model="form.password"
          autocomplete="current-password"
          class="h-10 pl-9 pr-10"
          name="password"
          placeholder="Enter your password"
          :type="showPassword ? 'text' : 'password'"
        />
        <Button
          :aria-label="showPassword ? 'Hide password' : 'Show password'"
          class="absolute right-1 top-1/2 size-8 -translate-y-1/2 text-muted-foreground"
          size="icon"
          type="button"
          variant="ghost"
          @click="showPassword = !showPassword"
        >
          <EyeOff v-if="showPassword" class="size-4" />
          <Eye v-else class="size-4" />
        </Button>
      </div>
    </div>

    <div class="flex items-center justify-between gap-4">
      <div class="flex items-center gap-2">
        <Checkbox id="remember-me" v-model="form.rememberMe" name="rememberMe" />
        <Label class="text-muted-foreground" for="remember-me">Remember me</Label>
      </div>
    </div>

    <Button class="h-10 w-full" :disabled="!canSubmit || isSubmitting" type="submit">
      <LoaderCircle v-if="isSubmitting" class="size-4 animate-spin" />
      <span>{{ isSubmitting ? 'Signing in' : 'Sign in' }}</span>
    </Button>
  </form>
</template>
